# LayerMask no Editor [Unity] (Escrevendo...)

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
O valor é atualizado, porém percaba quando iniciamos o jogo:
>
![teste](https://user-images.githubusercontent.com/60985347/139671265-923f0d88-04bc-4ffa-8474-9a48484c73ef.gif)
>
O valor é resetado, mas por quê? Simples, a data das váriaveis é salva dentro dos GameObjets e não dentro dos scripts, e como o script do Editor não pode ficar dentro de um GameObject (porque a Unity não permite) ele não salvará a data das variáveis alteradas, por isso que o valor é resetado, mas então como nós armazenamos o valor dessa variável? A resposta é simples também, basta nós criarmos uma variável dentro do Script `Cube.js` que irá armazenar os valores da variável do Editor.
>
Dentro do Cube.js
```cs
public LayerMask layer; // <--- Já constava antes
public int maskField;
```
