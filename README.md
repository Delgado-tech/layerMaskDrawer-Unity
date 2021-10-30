# LayerMask no Editor [Unity] 

  Quando estamos criando um jogo na Unity é muito comum criarmos várias váriaveis de controle, e isso pode acabar fazendo com que o nosso Inspetor de objeto fique muito desorganizado e poluído, e para resolvermos isso podemos criar um script a parte extendendo á classe Editor que possíbilita montar e organizar o Inspetor ao nosso gosto. 
  Vamos criar um Editor para uma classe criada chamada `Cube`, essa classe tem apenas uma váriavel de LayerMask dentro dela e nada mais (`public LayerMask layer;`). 
  
  CubeEditor.cs:
  ```cs
using System.Collections.Generic; // <--- necessário pois será utilizado uma váriavel de List
using UnityEngine; // <--- Obrigatório
using UnityEditor; // <--- Obrigatório
using UnityEditorInternal; // <--- necessário para utilizar a classe InternalEditorUtility, terá mais foco á frente


[CustomEditor(typeof(Cube))] // <--- Esse editor é referente à uma classe chamada Cube (será uzada ela como exemplo)
public class CubeEditor : Editor {

  ```
<br>
  Dentro da classe do Editor nos sobrescrevemos o metódo referente á do inspetor:
  
  ```cs 
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI(); //<--- Representa os elementos que estão sendo mostrados no Inspetor padrão, basta comentá-lo para mostrar apenas o que está presente aqui, use para debugar seus valores se quiser

  ``` 
  Nós vamos usar váriaveis e metódos que permitirão fazermos o inspetor ao nosso gosto, como o `EditorGUILayout` que nos disponibiliza vários InputFields (de float, string, etc), porém não disponibilizará valores referentes de nenhuma função da Unity, você precisará dá-los você mesmo, basicamente você estará recebendo a carcaça do Input, popup, textbox, etc. Para você mostrar os valores que serão sobrescritos, você precisa pega-los por uma váriavel e coloca-los como valor nos Inputs do Editor. Para pegar os valores da classe Input basta Instância-la.
  ```cs
  Cube cube = (Cube)target;
  ```
  

