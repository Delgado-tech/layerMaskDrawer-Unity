<span id="header"></span>
<h1 align="center"> LayerMask no Editor [Unity] (Escrevendo...) </h1>

> <span align="justify">Nesse artigo eu irei mostrar como podemos criar um popup de LayerMask pelo editor, eu irei explicar passo a passo de como fazer isso, e do que está acontecendo no código, caso você queira apenas o código, entre na pasta `Scripts` desse repositório e copie-o e adapite-o ao seu projeto. </span>
<br>
<br>
<div align="center"><img src="https://user-images.githubusercontent.com/60985347/139725469-2f2588b8-860e-4488-85f5-b2d3d98cb183.png" width="20%"></div>

<span id="sumario"></span>
# Sumário
- **<a href="#topico1">1. Introdução e preparando o ambiente</a>**
- **<a href="#topico2">2. Criando o popup</a>**
- **<a href="#topico3">3. Entendendo possíveis erros</a>**
- **<a href="#topico4">4. Consertando o popup</a>** <br>
✢ **<a href="#4_subtopico1">4.1. Metódo 1</a>** <br>
✢ **<a href="#4_subtopico2">4.2. Metódo 2</a>**
- **<a href="#footer">Rodapé</a>** <br>
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
  `GUIContent` é referente ao nome e a descrição do Field; <br>
  `int` é referente as opções selecionadas no popup; <br>
  `string[]` é referente à um Array das opções selecionaveis. <br>
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
//podemos alterar o valor diretamente com esse método, então não tem a necessidade da váriavel de instância "maskField" no Editor
cube.layer = EditorGUILayout.MaskField(new GUIContent("Layer", "escolha uma layer"), cube.maskField, layers);
cube.maskField = cube.layer;
```
<br>

<span id="4_subtopico2"></span>
### - Metódo 2
>


<span id="footer"></span>
<div align="center"><a href="#header">Voltar ao topo</a></div>

<div align="center"><img src="https://user-images.githubusercontent.com/60985347/139723592-63c80e23-fdaa-4ffc-ae79-0762993afee7.png" width="20%"></div>
