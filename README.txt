# SimuladorNorma

Esse � o projeto de um simulador da execu��o
e calculador de equival�ncia de programas
para a m�quina NORMA, de acordo com as defini��es
e a sintaxe do livro do Prof. Tiaraju Diverio.

## Modelo l�gico

H� tr�s entidades abstratas utilizadas na implementa��o:
Machine, Program e Computation. Para generalizar ao
m�ximo o c�digo, aumentando as possibilidades de reutiliza��o,
a maior parte das informa��es passadas a essas entidades
s�o strings que devem ent�o ser parseadas.

Machine � a implementa��o da m�quina, baseada na sua 
defini��o formal.

Program � um programa em si, de diversos tipos poss�veis.
A �nica restri��o para todos os tipos � que, dada uma
m�quina e um valor de entrada, ele consiga gerar uma
Computation correspondente. 

Parte da execu��o espec�fica para um tipo de programa 
pode (e deve) ser definida nas suas classes, assim
se utiliza polimorfismo para abstrair a estrutura do
programa durante sua execu��o.

Computation � encarregada do controle da execu��o do
programa, mantendo um registro de todos os estados
atingidos e do resultado obtido no final. Implementa
o padr�o de Observador.

### M�quina

Definida genericamente na interface IMachine, �
baseada na defini��o formal.

O conjunto de valores aceito pela mem�ria � definido
de maneira impl�cita: a �nica maneira de ver o estado
atual � pedindo uma representa��o em string (para uma
m�quina gen�rica).
