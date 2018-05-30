Trabalho da disciplina Sistemas de Comunicação II do curso de Engenharia de Computação da FAESA.

Codificação Lampel-Ziv


O programa tem dois botões.
Um que compacta um arquivo texto escolhido, gerando um novo arquivo e exibindo a taxa de compressão.
E outro que descompacta o arquivo escolhido, gerando o arquivo texto original.


Descrição do professor:
Implemente em linguagem de programação à sua escolha um compactador e um descompactador que implemente o algoritmo Lempel-Ziv conforme explicado em sala de aula. O compactador deve receber um arquivo como entrada e gerar um arquivo de saída compactado adicionando a extensão .lz. Lembre-se que a posição 0 do dicionário é reservada para o símbolo vazio, ou seja, ausência de símbolo. Já que não temos como saber quais bytes ocorrem (ou não) no arquivo de entrada, a representação do símbolo que diferencia uma sequência existente no dicionário de uma nova sequência deve ser representado pelo próprio byte (sem mapeá-lo em uma sequência mais curta de bits).
Após a confecção do programa, compacte 10 arquivos de tamanhos variados (entre 1Kbyte e 10Mbytes) e obtenha a taxa de compressão obtida. Calcule a taxa de compressão do Lempel-Ziv (comparando os tamanhos dos arquivos original e compactado. Escreva um pequeno relatório com esses resultados.
