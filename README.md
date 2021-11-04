<span id="header"></span>
<h1 align="center"> LayerMask no Editor [Unity] </h1>

> <span align="justify">Nesse artigo eu irei mostrar como podemos criar um popup de LayerMask pelo editor, eu irei explicar passo a passo de como fazer isso, e do que está acontecendo no código, caso você queira apenas o código, entre na pasta `Scripts` desse repositório e copie-o e adapite-o ao seu projeto. </span>
<br>
<br>
<div align="center"><img src="https://user-images.githubusercontent.com/60985347/139725469-2f2588b8-860e-4488-85f5-b2d3d98cb183.png" width="20%"></div>

<span id="sumario"></span>
# Sumário
> - **<a href="#topico1">1. Introdução e preparando o ambiente</a>**
> - **<a href="#topico2">2. Criando o popup</a>**
> - **<a href="#topico3">3. Entendendo possíveis erros</a>**
> - **<a href="#topico4">4. Consertando o popup</a>** <br>
> ✢ **<a href="#4_subtopico1">4.1. Metódo 1</a>** <br>
> ✢ **<a href="#4_subtopico2">4.2. Metódo 2</a>** ← A resolução do script presente no repositório
> - **<a href="#conclusao">5. Conclusão</a>**
>
>- **<a href="#footer">Rodapé</a>** <br>
#

<div align="center"><img src="https://user-images.githubusercontent.com/60985347/139723592-63c80e23-fdaa-4ffc-ae79-0762993afee7.png" width="20%"></div>
<br>
<br>
<span id="topico1"></span>

## Introdução e preparando o ambiente
>
  Quando estamos criando um jogo na Unity é muito comum criarmos várias váriaveis de controle, e isso pode acabar fazendo com que o nosso Inspetor de objeto fique muito desorganizado e poluído, e para resolvermos isso podemos criar um script a parte extendendo á classe Editor que possíbilita montar e organizar o Inspetor ao nosso gosto. 
  Vamos criar um Editor para uma classe criada chamada `Cube`, essa classe tem apenas uma váriavel de LayerMask dentro dela e nada mais (`public LayerMask layer;`). 
  
  CubeEditor.cs:
  ```cs
using System.Collections.Generic; // <--- necessário pois será utilizado uma váriavel de List
using UnityEngine; // <--- Obrigatório
using UnityEditor; // <--- Obrigatório
using UnityEditorInternal; // <--- necessário para utilizar a classe InternalEditorUtility, terá mais foco á frente


[CustomEditor(typeof(Cube))] // <--- Esse editor é referente à uma classe chamada Cube (será usada ela como exemplo)
public class CubeEditor : Editor {

  ```
>
  Dentro da classe do Editor nos sobrescrevemos o metódo referente á do inspetor:
  
  ```cs 
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI(); //<--- Representa os elementos que estão sendo mostrados no Inspetor padrão, basta comentá-lo para mostrar apenas o que está presente aqui, use para debugar seus valores se quiser

  ``` 
  Nós vamos usar variáveis e metódos que permitirão fazermos o inspetor ao nosso gosto, como o `EditorGUILayout` que nos disponibiliza vários InputFields (de float, string, etc), porém não disponibilizará valores referentes de nenhuma função da Unity, você precisará dá-los você mesmo, basicamente você estará recebendo a carcaça do Input, popup, textbox, etc. Para você vincular os valores do Editor com a classe `Cube` você precisa acessar esses valores e modifica-los quando for alterado algo no Inspetor sobrescrito, para isso Instâncie a classe `Cube`. 
  ```cs
  Cube cube = (Cube)target;
  ```
  Agora nos podemos acessar a variável da LayerMask e alterá-la quando o Inspetor sobrescrito for mudado (via `cube.layer`).
<br>

<div align="center"><img src="https://user-images.githubusercontent.com/60985347/139723592-63c80e23-fdaa-4ffc-ae79-0762993afee7.png" width="20%"></div>

<span id="topico2"></span>
## Criando o popup
> <a href="#sumario"># retornar ao sumário</a>
>
  Para criar um popup estilo do LayerMask, nos usamos `EditorGUILayout.MaskField(GUIContent, int, string[]);`. <br> 
  ✢ `GUIContent` é referente ao nome e a descrição do Field; <br>
  ✢ `int` é referente as opções selecionadas no popup; <br>
  ✢ `string[]` é referente à um Array das opções selecionaveis. <br>
>
Vamos criar uma variável de instância que irá armazenar as opções selecionadas.
```cs
int maskField;
```
>
Agora só precisamos pegar os valores que terão dentro do popup, no caso os mesmo da LayerMask, por isso nós chamamos o namespace `UnityEditorInternal`, nos conseguimos pegar as camadas pela chamada da variável `InternalEditorUtility.layers`, ela retorna um array de strings com todos os nomes das layers existentes.
```cs
EditorGUILayout.MaskField(new GUIContent("Layer", "escolha uma layer"), maskField, InternalEditorUtility.layers);
// new GUIContent(string label, string descrição)
```
![image](https://user-images.githubusercontent.com/60985347/139606457-d18b5175-10b8-4c3c-b3e2-1c8215c5e6b2.png)
> obs: caso você não tenha comentado o `base.OnInspectorGUI();` aparecerá as variáveis do script padrão (cube.js), por isso tem dois campos chamado Layer.

Se formos no inspetor conseguimos ver o popup com os valores do LayerMask que criamos, porem não é possível escolher outro valor no popup, pois o valor escolhido não é atualizado no script quando você altera ele pelo inspetor, para corrigir isso, basta fazer ele receber o retorno do método MaskField (retorna os valores escolhidos).
```cs
maskField = EditorGUILayout.MaskField(new GUIContent("Layer", "escolha uma layer"), maskField, InternalEditorUtility.layers);
``` 
<br>

<div align="center"><img src="https://user-images.githubusercontent.com/60985347/139723592-63c80e23-fdaa-4ffc-ae79-0762993afee7.png" width="20%"></div>

<span id="topico3"></span>
## Entendendo possíveis erros
> <a href="#sumario"># retornar ao sumário</a>
>
Feito a alteração a cima podemos ver que o valor é atualizado com sucesso, porém percaba quando iniciamos o jogo:
>
![teste](https://user-images.githubusercontent.com/60985347/139671265-923f0d88-04bc-4ffa-8474-9a48484c73ef.gif)
>
O valor é resetado, mas por quê? Simples, a data das váriaveis é salva dentro dos GameObjets e não dentro dos scripts, e como o script do Editor não pode ficar dentro de um GameObject (porque a Unity não permite) ele não salvará a data das variáveis alteradas, por isso que o valor é resetado, mas então como nós armazenamos o valor dessa variável? A resposta é simples também, basta nós criarmos uma variável dentro do Script `Cube.js` que irá armazenar os valores da variável do Editor.
>
Dentro do Cube.js
```cs
public LayerMask layer; // <--- Já constava antes
[HideInInspector] public int maskField;
```
Voltando para o script `CubeEditor.js`, nós dizemos para função MaskField usar o valor do scirpt `Cube.js`, e logo depois atualizamos esse valor com o valor atual do Editor.
```cs
maskField = EditorGUILayout.MaskField(new GUIContent("Layer", "escolha uma layer"), cube.maskField /*<--- alterado*/, InternalEditorUtility.layers);
cube.maskField = maskField;
```
>
E agora quando formos testar vai estar funcionando perfeitamente. Agora precisamos fazer com que o resultado das opções escolhidas seja recebido pela variável da LayerMask, o metódo MaskField retorna um int das opções escolhidas assim como o valor da LayerMask, você deve estar pensando "basta referênciar o valor recebido do método com o do LayerMask", e você está certo! Mas antes vamos ver o que acontece quando fazemos isso, primeiro vamos fazer essa referência.
>
```cs
cube.layer = maskField;
```
>
![teste2](https://user-images.githubusercontent.com/60985347/139688479-72069842-e67f-426b-83d6-155b4d162bec.gif)
>
Você deve estar falando "Ué? Por que quando eu estou escolhendo uma camada em um, está sendo escolhida outra camada no outro?", bem a razão disso acontecer é porque o LayerMask ele pega as layers existêntes da Unity.
>
![image](https://user-images.githubusercontent.com/60985347/139693597-c2ce4aad-290c-48ca-83fb-2b9dfc8bb3b8.png)
>
Ele contabiliza todas as layers incluindo as vazias porém não mostra elas no popup de LayerMask. Já o `InternalEditorUtility` só retorna os valores não vazios do LayerMask, por isso esse erro está acontecendo.
>

<div align="center"><img src="https://user-images.githubusercontent.com/60985347/139723592-63c80e23-fdaa-4ffc-ae79-0762993afee7.png" width="20%"></div>

<span id="topico4"></span>
## Consertando o popup
> <a href="#sumario"># retornar ao sumário</a>
>
Com relação ao erro mencionado a cima temos algumas formas de arrumar isso, eu irei comentar dois metódos diferentes que podemos optar, um é mais simples e com menos código, já o outro é maior e tem um resultado um pouco melhor que o anterior.
>
<span id="4_subtopico1"></span>
### - Método 1
>
Uma possível saída seria criar um Array de strings de tamanho 32 (que é o número maxímo de layers que a Unity permite seu projeto ter) e fazer um `for` para colocar as camadas na posição correta, vamos tentar.
>
```cs
string[] layers = new string[32]; //Array que irá armazenar os nomes das layers
for (int i = 0; i < layers.Length; i++) { //vai percorrer cada índice do array
  foreach (var layer in InternalEditorUtility.layers) { //irá percorrer cada string do array InternalEditorUtility.layers
    if (layer == LayerMask.LayerToName(i)) { //LayerMask.LayerName() é um método que você passa um índice de uma layer e retorna o nome dela
      layers[i] = layer; //caso a string atual (layer) seja igual a string das layers da Unity, é adicionada no array layers essa string
    }
  }
}

maskField = EditorGUILayout.MaskField(new GUIContent("Layer", "escolha uma layer"), cube.maskField, layers /*<--- alterado*/);
```
>Resultado:
>
>![metodo1](https://user-images.githubusercontent.com/60985347/139709507-93c2370c-7a8a-456f-8ab4-64573297ebc9.gif)
>
O código está funcional! Porém visualmente não está igual, as layers vazias são aquelas linhas que você pode ver no popup de baixo, infelizmente não é possível não inclui-las no popup. <br>
Você pode adotar esse método com esse código mais simples e menor que irá funcionar perfeitamenta, mas se você quiser que fique funcional __***e***__ visualmente igual, teremos que optar por um método diferente, antes de ver qual é esse método, vou comentar um ajuste que quem queira optar por esse primeiro método pode fazer:
>
```cs
maskField = EditorGUILayout.MaskField(new GUIContent("Layer", "escolha uma layer"), cube.maskField, layers);

if (maskField != cube.maskField) { // altera o valor apenas se for mudado no inspetor
  cube.maskField = maskField;
  cube.layer = maskField;
}

if (maskField != cube.layer) { // caso o valor seja mudado internamente precisará atualiza-lo visualmente no inspetor
  cube.maskField = cube.layer;
}
```
> <a href="#conclusao"># ir para a Conclusão</a>
<br>

<span id="4_subtopico2"></span>
### - Metódo 2
>
Apartir de um código diferente um pouco maior, nós conseguimos consertar o visual do resultado anterior, mas antes de nós começarmos a faze-lo devemos entender como que funciona o retorno do LayerMask (MaskField tem o mesmo tipo de retorno também). <br>
Você já se perguntou por que o retorno do LayerMask.value é apenas um int e não um array de int's já que você pode selecionar multiplas camadas? Para entendermos o por que disso, vamos debugar o valor de LayerMask e ver o retorno dele (`Debug.Log(layer.value)`).

> Faça isso apenas se quiser testar por você mesmo, mas você terá o mesmo resultado do gif abaixo.
![debuging](https://user-images.githubusercontent.com/60985347/139731230-f54cc78b-e9cf-419b-b53f-009b3e2d37ab.gif)

Se analizarmos com exceção do `Nothing` e `Everything` (que não são camadas de verdade e sim apenas opções de seleção rápida) conseguimos perceber um padrão a partir da primeira camada, `Default = 1` (layer 0), `TransparentFX = 2` (layer 1), `Ignore Raycast = 4` (layer 2), `Water = 16` (layer 4 (não apareceu no gif, mas teve esse retorno)), Que tipo de calcúlo está sendo usado ali? Se você respondeu potênciação acertou na mosca! Os indíces estão servindo como expoentes de base 2.

![image](https://user-images.githubusercontent.com/60985347/139734070-116026f3-1995-4972-b454-214bd5cc695b.png)

Basicamente quando escolhemos uma opção é esse o retorno, e quando escolhemos mais de uma, nós temos a soma das camadas como retorno `(Ex: Layer 1 (vale 2) + Layer 3 (vale 4) = 6)`, e o resultado dessa soma é única para cada combinação de layers selecionadas, por isso a Unity tem um limite de 32 layers, porque a cima disso os retornos terão números muito altos que o tipo int não suporta, se for somado o resultado das layers de 0 até a 30 você terá o retorno de `2147483647` que é exatamente o limite que o int aceita, mas peraí e a layer 31 (a última)? Se somar com ela o valor vai ultrapassar, não? Na lógica sim, por isso que o valor dela é diferente, ao invés dela valer `2^31` ela vale o limite negativo do int `-2147483647`, fazendo assim todas as suas combinações terem retorno negativo quando escolhida. 
> Obs: As opções Nothing e Everything correspondem respectivamente aos valores, 0 e -1.

Certo, agora sabendo isso, nós devemos pegar o valor das opções escolhidas do MaskField (que é a somatória delas, porém que não está na ordem com relação aos valores das layers da Unity) e converte-lo para um valor igual do input de LayerMask do script `Cube`. Para isso devemos desenvolver uma formúla para sabermos quais foram as layers escolhidas para dar esse valor (já que o retorno de MaskField só retorna int e não um Array de string com o nome das layers escolhidas).

Vamos começar criando as váriaveis necessárias, ao todo são 5 váriaveis, 2 de instância e 3 locais.

```cs
// variáveis de instância
int convertedValue;
List<string> layers;
```
✢ `convertedValue` será a variável que irá armazenar o valor convertido e passa-lo para variável `layer` do script `Cube`. <br>
✢ `layers` armazenará o nome das layers escolhidas.


```cs
// variáveis locais
int tempVal = maskField;
int x = 1;
int l = 0;
```
✢ `tempVal`, vamos utilizar essa variável para armazenar o valor de maskField, pois esse valor será alterado e não queremos que isso aconteça com a variável principal (se utilizarmos o valor de maskField direto iria acontecer do popup travar na opção `Nothing`, você vai entender o por que com a formúla a baixo). <br>
✢ `x` será o valor decrescido da váriavel `tempVal`. <br>
✢ `l`, corresponde ao índice da layer com aquele valor, ela será adicionada na List de `layers`.

> A formúla funcionará dessa forma: será utilizado um `while` que irá se manter em loop até `tempVal` tiver o valor de 0, a cada chamada a variável `x` multiplicará ela mesma por 2, assim ela corresponderá ao valor de layer (lembrando que o valor das layers é dado usando o índice delas como expoente de base 2), e caso o próximo incremento de `x` ultrapassar o valor de `tempVal` quer dizer que achamos o índice de uma layer, então decrementamos (ou incrementamos caso a layer 31 esteja selecionada) `x` de `tempVal`, e resetamos o valor de `x`, assim o loop de while será repetido até 0, com a variável `l` nós saberemos que índice foi esse, e poderemos usa-lo com o código `InternalEditorUtility.layers[l]` e pegar o nome de sua camada e adiciona-la na List `layers`.
>
```cs
if (maskField != 0 && maskField != -1) { // caso o valor for 0 ou -1 não precisará chamar esse código, já que não é possível escolher outra opção quando esses valores forem escolhidos
  convertedValue = 0; // será preciso que esse valor seja resetado caso entre nesse if
  layers = new List<string>(); // será preciso que esse valor seja resetado caso entre nesse if

  int tempVal = maskField;
  int x = 1;
  int l = 0;

  while (tempVal != 0) {
  
    if (tempVal > 0 && x * 2 > tempVal) {
       layers.Add(InternalEditorUtility.layers[l]); // adiciona na lista o nome da layer correspondente ao índice "l"
       tempVal -= x;
       x = 1;
       l = 0;
       continue;
    }
    
    if (tempVal < 0 && x * 2 < tempVal) { // caso a layer 31 for escolhida
       layers.Add(InternalEditorUtility.layers[l]); // adiciona na lista o nome da layer correspondente ao índice "l"
       tempVal += x;
       x = 1;
       l = 0;
       continue;
    }
    
    x *= 2;
    l++;
  }
  
  /*
  /////////////////////////////
  Próximo passo ficará aqui
  /////////////////////////////
  */
  
} else {
  convertedValue = maskField; // se o valor for 0 ou -1
}

cube.maskField = maskField; // <--- essa atribuição lá do começo foi realocada para cá
cube.layer = convertedValue; // <--- irá receber o valor convertido
```
Agora que sabemos os nomes das layers que estão sendo utilizadas pelo metódo MaskField podemos conseguir converter esse valor para o das layers da Unity com o método `LayerMask.GetMask(string[])`, ele basicamente retorna a soma dos valores das layers informadas no Array.

```cs
// essa linha será colocado no campo indicado a cima
convertedValue = LayerMask.GetMask(layers.ToArray());
```
E *tcharam*! Temos uma cópia exata de um popup de LayerMask.
> Resultado: <br>
> ![resultado](https://user-images.githubusercontent.com/60985347/139840821-5f03d114-1fed-4d28-aca9-07b159c3466e.gif) <br>
> Nota: Podemos dentro do script `Cube` colocar a tag `[HideInInspector]` ao lado de `public LayerMask layer` ou comentar a linha com `base.OnInspectorGUI();` no script do Editor para mostrar apenas o popup criado.

O código ainda não está finalizado, ainda precisamos fazer alguns ajustes nele para ele ficar otimizado e prático. <br>
Primeira coisa, podemos fazer com que apenas o código de conversão seja executado quando for alterado algum valor no popup, ao invés de executa-lo a todo momento.
```cs
if(maskField == cube.maskField) return; // se os valores continuarem iguais, não tem necessidades de executar a formula
// if (maskField != 0 && maskField != -1) {
``` 
Segunda coisa, toda essa formúla está para apenas um único popup, e se você quiser adicionar mais de um teria que ficar copiando e colando essa formúla, não seria prático e ficaria poluído o seu script, por isso podemos criar um script separado com uma classe estática e um metódo estático, assim nós só chamaremos esse metódo por essa classe ao invés de ficar copiando e colando esse código para cada popup.
<br>

EditorMethods.cs
```cs
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;

namespace EditorMethods { // podemos criar um namespace para só chamar essa função quando formos usa-la em algum script
  public static class LayerMaskDrawer {
  
    static List<string> layers; // <--- Variável realocada para cá
    
      public static void Draw(int maskField, ref int lateMaskField, ref int convertedValue, ref LayerMask mask) { // palavra-chave ref = o valor recebido quando for alterado aqui será alterado na própria variável passada ao invés de receber apenas a cópia do valor
        ////////////////Metódo//////////////////
      }     
  }
}
```
Agora podemos recortar a nossa formula e colá-la aqui, apenas vamos mudar os nomes de duas variáveis, `cube.maskField = lateMaskField` e `cube.layer = mask`.
```cs
if (maskField == lateMaskField) return;


if (maskField != 0 && maskField != -1) {
  convertedValue = 0;
  layers = new List<string>();

  int tempVal = maskField;
  int x = 1;
  int l = 0;

while (tempVal != 0) {
  if (tempVal > 0 && x * 2 > tempVal) {
    layers.Add(InternalEditorUtility.layers[l]);
    tempVal -= x;
    x = 1;
    l = 0;
    continue;
  }


  if (tempVal < 0 && x * 2 < tempVal) {
    layers.Add(InternalEditorUtility.layers[l]);
    tempVal += x;
    x = 1;
    l = 0;
    continue;
  }

  x *= 2;
  l++;
}

convertedValue = LayerMask.GetMask(layers.ToArray());

} else {
  convertedValue = maskField;
}

lateMaskField = maskField;
mask = convertedValue;
```
Antes de atualizarmos o script `CubeEditor.js`, vamos criar mais um metódo, esse metódo a cima que fizemos é apenas para quando você alterasse um valor pelo inspetor, mas e se você mudasse esse valor internamente? O inspetor não iria mudar, pois só fizemos a conversão do `Editor>>>Cube.js` e não do `Cube.js>>>Editor`, para mudar é simples, basta nós reutilizarmos a formula que fizemos só que para converter o LayerMask para MaskField.
```cs
static void InternalMaskUpdate(ref int convertedValue, ref int lateMaskField, ref LayerMask mask) {
  if (mask != 0 && mask != -1) {
  layers = new List<string>();

  int tempVal = mask;
  int x = 1;
  int l = 0;
  while (tempVal != 0) {

    if (tempVal > 0 && x * 2 > tempVal) {
      layers.Add(LayerMask.LayerToName(l)); // LayerMask.LayerToName(int) - Passa o índice da camada e retorna o nome dela
      tempVal -= x;
      x = 1;
      l = 0;
      continue;
    }


    if (tempVal < 0 && x * 2 < tempVal) {
      layers.Add(LayerMask.LayerToName(l));
      tempVal += x;
      x = 1;
      l = 0;
      continue;
    }

    x *= 2;
    l++;
  }

  lateMaskField = 0; // variável referencia cube.maskField, ela que manda em qual opção visualmente está selecionada no inspetor
  for (int i = 0; i < InternalEditorUtility.layers.Length; i++) { // percorre todas as layers do InternalEditorUtility.layers
    foreach (var layer in layers) { // percorre cada layer registrada na List de strings
      if (InternalEditorUtility.layers[i] == layer) { // caso o nome da layer do InternalEditor coincida com a da List
        lateMaskField += (int)Mathf.Pow(2, i); // adiciona o valor daquela layer para cube.maskField, para pegar o valor da camada, usa-se o seu índice como exponte de base 2, como o retorno é float você casta o retorno como (int)
       }
     }
   }

  } else {
    lateMaskField = mask; // caso o valor alterado internamente for 0 ou -1
  }

  convertedValue = mask; // atualiza o valor convertido para ficar igual o de mask (ou cube.layer)
  }
}
```
Voltando para o metódo Draw:
```cs
public static void Draw(int maskField, ref int lateMaskField, ref int convertedValue, ref LayerMask mask) {

if (maskField == lateMaskField && mask != convertedValue) { //caso o valor de maskField não tenha sido alterado, mas o valor de mask tenha
  InternalMaskUpdate(ref convertedValue, ref lateMaskField, ref mask);
  return;
}

if (maskField == lateMaskField) return;

``` 
<br>

Agora podemos voltar para o script `CubeEditor.js` e fazer as últimas alterações.
<br>
CubeEditor.js
```cs
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using EditorMethods; // como criamos um namespace para chamar apenas quando precisarmos do metódo, usamos o using para chamar ele agora

[CustomEditor(typeof(Cube))]
public class CubeEditor : Editor {

  int maskField;
  int convertedValue;
  //Foi realocado List<string> layers para o LayerMaskDrawer

  public override void OnInspectorGUI() {
    base.OnInspectorGUI();

    Cube cube = (Cube)target;	


    maskField = EditorGUILayout.MaskField(new GUIContent("Layer", "escolha uma layer"), cube.maskField, InternalEditorUtility.layers);
    LayerMaskDrawer.Draw(maskField, ref cube.maskField, ref convertedValue, ref cube.layer); // <--- Metódo chamado em baixo do metódo MaskField

  }
}
```
E pronto! Você pode usar esse metódo que fizemos para qualquer editor que criar que ele estará funcionando perfeitamente, porém caso você queira criar mais de um popup você teria que criar três variáveis para cada um que criar, e isso não seria bom, porque poluíria muito o código. A melhor forma para contornarmos essa situação é substituir as variáveis de instância por Arrays, assim usamos a mesma variável, porém só chamamos o seu índice.
<br>

CubeEditor.js
```cs
int PopupsCount = 2; // como todo array precisa de um tamanho podemos controlar por essa variável o tamanho dele
int[] maskField;
int[] convertedValue;
```
Cube.js
```cs
[HideInInspector] public int[] maskField;
```
<br>

Agora precisamos declarar o seus valores dentro de `OnInspectorGUI()`;
```cs
//Cube cube = (Cube)target; //<--- apenas referencia

maskField = new int[PopupsCount]; // <--- olha aí o nosso demilitador (PopupsCount)
convertedValue = new int[PopupsCount];
if (cube.maskField.Length == 0) cube.maskField = new int[PopupsCount]; // como que cube.maskField é um valor que não pode resetar, chamamos ele apenas caso ele não tenha sido iniciado.
```
E para finalizar basta atualizar os valores nos metódos:
```cs
maskField[0] = EditorGUILayout.MaskField(new GUIContent("Layer", "escolha uma layer"), cube.maskField[0], InternalEditorUtility.layers);
LayerMaskDrawer.Draw(maskField[0], ref cube.maskField[0], ref convertedValue[0], ref cube.layer); // <--- Metódo chamado em baixo do metódo MaskField
```
Agora sim! O código está 100%! Caso queira criar um outro LayerMask, é só copiar essas duas linhas a cima e aumentar o valor do índice, e trocar a layer que está sendo referenciada por outra (`ref cube.layer -> ref cube.outraLayer`).

<div align="center"><img src="https://user-images.githubusercontent.com/60985347/139723592-63c80e23-fdaa-4ffc-ae79-0762993afee7.png" width="20%"></div>

<span id="conclusao"></span>
## Conclusão
Existem diversos modos de fazer esse código, mas eu quis compartilhar esse metódo que eu fiz, caso ache algum erro no código ou uma forma de melhora-lo não hesite de fazer um Pull Request, desde já muito obrigado por me acompanhar nesse artigo aqui, até um próximo tutorial ou repositório e falou!

<span id="footer"></span>
<div align="center"><a href="#header">Voltar ao topo</a></div>

<div align="center"><img src="https://user-images.githubusercontent.com/60985347/139723592-63c80e23-fdaa-4ffc-ae79-0762993afee7.png" width="20%"></div>
