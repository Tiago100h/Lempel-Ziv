using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Lempel_Ziv
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int Contador = 0;
        public string ContadorBinario
        {
            get
            {
                return Convert.ToString(Contador, 2);
            }
        }

        /// <summary>
        /// Compacta o arquivo .txt escolhido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Compactar_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "TXT files|*.txt"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var tamanhoInicial = new System.IO.FileInfo(ofd.FileName).Length;

                // Lendo arquivo
                string arquivo;
                using (var sr = new System.IO.StreamReader(ofd.FileName))
                {
                    arquivo = sr.ReadToEnd();
                }

                // Variáveis para controlar o loop e o dicionário
                Contador = 0;
                var indice = 0;
                var tamanho = 1;
                var sequenciaNova = string.Empty;
                Dicionario ultimaSequenciaEncontrada = null;

                // Criando o dicionário e adicionando o simbolo vazio
                var dicionario = new List<Dicionario>
                {
                    new Dicionario { PalavraCodigo = ContadorBinario, Sequencia = "" }
                };

                // Criando variável para armazenar o conteúdo com bits e bytes
                var sequenciaCodificada = new List<SequenciaCodificada>();

                // Percorrendo arquivo
                while (indice < arquivo.Length && indice + tamanho < arquivo.Length + 1)
                {
                    sequenciaNova = arquivo.Substring(indice, tamanho);
                    var sequenciaDicionario = dicionario.Find(d => d.Sequencia == sequenciaNova);

                    if (sequenciaDicionario is null)
                    {
                        if (ultimaSequenciaEncontrada is null)
                        {
                            for (int i = 0; i < dicionario.Last().PalavraCodigo.Length; i++)
                            {
                                sequenciaCodificada.Add(new SequenciaCodificada { Sequencia = dicionario.First().PalavraCodigo, Bit = true });
                            }
                            sequenciaCodificada.Add(new SequenciaCodificada { Sequencia = sequenciaNova, Bit = false });
                            Contador++;
                            dicionario.Add(new Dicionario { PalavraCodigo = ContadorBinario, Sequencia = sequenciaNova });
                            indice += tamanho;
                            tamanho = 1;
                        }
                        else
                        {
                            for (int i = ultimaSequenciaEncontrada.PalavraCodigo.Length; i < dicionario.Last().PalavraCodigo.Length; i++)
                            {
                                sequenciaCodificada.Add(new SequenciaCodificada { Sequencia = dicionario.First().PalavraCodigo, Bit = true });
                            }
                            sequenciaCodificada.Add(new SequenciaCodificada { Sequencia = ultimaSequenciaEncontrada.PalavraCodigo, Bit = true });
                            sequenciaCodificada.Add(new SequenciaCodificada { Sequencia = sequenciaNova.Last().ToString(), Bit = false });
                            Contador++;
                            dicionario.Add(new Dicionario { PalavraCodigo = ContadorBinario, Sequencia = sequenciaNova });
                            indice += tamanho;
                            tamanho = 1;
                        }
                        ultimaSequenciaEncontrada = null;
                        sequenciaNova = string.Empty;
                    }
                    else
                    {
                        ultimaSequenciaEncontrada = sequenciaDicionario;
                        tamanho++;
                    }
                }

                // Última sequência
                if (!string.IsNullOrWhiteSpace(sequenciaNova))
                {
                    if (ultimaSequenciaEncontrada is null)
                    {
                        for (int i = 0; i < dicionario.Last().PalavraCodigo.Length; i++)
                        {
                            sequenciaCodificada.Add(new SequenciaCodificada { Sequencia = dicionario.First().PalavraCodigo, Bit = true });
                        }
                        sequenciaCodificada.Add(new SequenciaCodificada { Sequencia = sequenciaNova, Bit = false });
                    }
                    else
                    {
                        for (int i = ultimaSequenciaEncontrada.PalavraCodigo.Length; i < dicionario.Last().PalavraCodigo.Length; i++)
                        {
                            sequenciaCodificada.Add(new SequenciaCodificada { Sequencia = dicionario.First().PalavraCodigo, Bit = true });
                        }
                        sequenciaCodificada.Add(new SequenciaCodificada { Sequencia = ultimaSequenciaEncontrada.PalavraCodigo, Bit = true });
                    }
                }

                // Removendo 0 inicial
                sequenciaCodificada.RemoveAt(0);

                // Substituindo bytes por bits
                var zerosEum = string.Join(string.Empty, sequenciaCodificada.Select(s => s.Bit ? s.Sequencia : Convert.ToString(s.Sequencia.ToCharArray()[0], 2).PadLeft(8, '0')));

                // Convertendo bits em bytes
                var bytes = new List<byte>();
                int j = 0;
                while (j + 8 <= zerosEum.Length)
                {
                    var oitoBits = zerosEum.Substring(j, 8);
                    var bitsEmByte = Convert.ToByte(oitoBits, 2);
                    bytes.Add(bitsEmByte);
                    j += 8;
                }

                // Adicionando zeros a esquerda do último bit para converter em byte, se necessário, e adicionando último byte referente a quantidade de zeros adicionados
                if (j + 8 == zerosEum.Length)
                {
                    bytes.Add(Convert.ToByte(0));
                }
                else
                {
                    var bitsRestantes = zerosEum.Length - j;
                    bytes.Add(Convert.ToByte(zerosEum.Substring(j, bitsRestantes).PadLeft(8, '0'), 2));
                    bytes.Add(Convert.ToByte(8 - bitsRestantes));
                }

                // Gerando arquivo
                var arquivoRetorno = ofd.FileName + ".lz";
                System.IO.File.WriteAllBytes(arquivoRetorno, bytes.ToArray());

                var tamanhoFinal = new System.IO.FileInfo(arquivoRetorno).Length;
                MessageBox.Show("Taxa de compressão: " + (decimal.Divide(tamanhoFinal, tamanhoInicial) * 100).ToString("0.00") + "%");
            }
        }

        /// <summary>
        /// Descompacta o arquivo .txt.lz escolhido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Descompactar_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Arquivos Lempel-Ziv|*.lz"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Lendo arquivo
                var arquivo = System.IO.File.ReadAllBytes(ofd.FileName);

                // Verificando quantidade de zeros adicionados na compactação
                var zerosAdicionados = Convert.ToInt32(arquivo.Last());

                // Removendo último byte
                var bytes = arquivo.ToList();
                bytes.Remove(bytes.Last());

                // Convertendo bytes em blocos de 8 bits
                var bits = string.Empty;
                foreach (var b in bytes)
                {
                    bits += Convert.ToString(b, 2).PadLeft(8, '0');
                }

                // Removendo zeros adicionados na compactação
                bits = bits.Remove(bits.Length - 8, zerosAdicionados);

                // Variáveis para controlar o loop e o dicionário
                Contador = 0;
                var indice = 0;
                var tamanho = 8;
                var sequenciaNova = string.Empty;
                var bytesConvertidos = new List<byte>();

                // Criando o dicionário e adicionando o simbolo vazio
                var dicionario = new List<Dicionario>
                {
                    new Dicionario { PalavraCodigo = ContadorBinario, Sequencia = "" }
                };

                // Percorrendo os bits
                while (indice < bits.Length)
                {
                    // Primeiro símbolo
                    if (dicionario.Count == 1)
                    {
                        sequenciaNova = bits.Substring(indice, tamanho);
                        Contador++;
                        dicionario.Add(new Dicionario { Sequencia = sequenciaNova, PalavraCodigo = ContadorBinario });
                        bytesConvertidos.Add(Convert.ToByte(sequenciaNova, 2));
                        indice += tamanho;
                    }
                    else
                    {
                        tamanho = dicionario.Last().PalavraCodigo.Length;
                        var palavraCodigo = bits.Substring(indice, tamanho);

                        // Removendo zeros a esquerda da palavra-código
                        palavraCodigo = palavraCodigo.TrimStart(new Char[] { '0' });

                        var sequenciaDicionario = string.Empty;
                        if (palavraCodigo.Length > 0)
                        {
                            sequenciaDicionario = dicionario.Find(d => d.PalavraCodigo == palavraCodigo).Sequencia;
                            for (int i = 0; i < sequenciaDicionario.Length; i+=8)
                            {
                                bytesConvertidos.Add(Convert.ToByte(sequenciaDicionario.Substring(i, 8), 2));
                            }
                        }
                        indice += tamanho;
                        tamanho = 8;
                        if (indice + tamanho <= bits.Length)
                        {
                            sequenciaNova = bits.Substring(indice, tamanho);
                            Contador++;
                            dicionario.Add(new Dicionario { Sequencia = string.Concat(sequenciaDicionario, sequenciaNova), PalavraCodigo = ContadorBinario });
                            bytesConvertidos.Add(Convert.ToByte(sequenciaNova, 2));
                        }
                        indice += tamanho;
                    }
                }

                // Convertendo bits em string
                var retorno = System.Text.Encoding.ASCII.GetString(bytesConvertidos.ToArray());

                // Gerando arquivo
                var arquivoRetorno = ofd.FileName + ".txt";
                System.IO.File.WriteAllText(arquivoRetorno, retorno);
                MessageBox.Show("Arquivo descompactado:" + Environment.NewLine + arquivoRetorno);
            }
        }
    }

    public class Dicionario
    {
        public string PalavraCodigo;
        public string Sequencia;
    }

    public class SequenciaCodificada
    {
        public bool Bit;
        public string Sequencia;
    }
}