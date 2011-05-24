# SimuladorNorma

Esse é o projeto de um simulador da execução
e calculador de equivalência de programas
para a máquina NORMA, de acordo com as definições
e a sintaxe do livro do Prof. Tiaraju Diverio.

## Modelo lógico

Há três entidades abstratas utilizadas na implementação:
Machine, Program e Computation. Para generalizar ao
máximo o código, aumentando as possibilidades de reutilização,
a maior parte das informações passadas a essas entidades
são strings que devem então ser parseadas.

Machine é a implementação da máquina, baseada na sua 
definição formal.

Program é um programa em si, de diversos tipos possíveis.
A única restrição para todos os tipos é que, dada uma
máquina e um valor de entrada, ele consiga gerar uma
Computation correspondente. 

Parte da execução específica para um tipo de programa 
pode (e deve) ser definida nas suas classes, assim
se utiliza polimorfismo para abstrair a estrutura do
programa durante sua execução.

Computation é encarregada do controle da execução do
programa, mantendo um registro de todos os estados
atingidos e do resultado obtido no final. Implementa
o padrão de Observador.

### Máquina

Definida genericamente na interface IMachine, é
baseada na definição formal.

O conjunto de valores aceito pela memória é definido
de maneira implícita: a única maneira de ver o estado
atual é pedindo uma representação em string (para uma
máquina genérica).
