# LayerMask no Editor [Unity] 

  Quando estamos criando um jogo na Unity é muito comum criarmos várias váriaveis de controle, e isso pode acabar fazendo com que o nosso Inspetor de objeto fique muito desorganizado e poluído, e para resolvermos isso podemos criar um script a parte extendendo á classe Editor que possíbilita montar e organizar o Inspetor ao nosso gosto. 
  ```cs
using System.Collections.Generic; // <--- necessário pois será utilizado uma váriavel de List
using UnityEngine; // <--- Obrigatório
using UnityEditor; // <--- Obrigatório
using UnityEditorInternal; // <--- necessário para utilizar a classe InternalEditorUtility, terá mais foco á frente


[CustomEditor(typeof(Cube))] // <--- Esse editor é referente à uma classe chamada Cube (será uzada ela como exemplo)
public class CubeEditor : Editor {

}
  ```
<br>
  Dentro da classe do Editor nos sobrescrevemos o metódo referente á do inspetor:
  ```cs 
  public override void OnInspectorGUI()
  ``` 
  Ela irá nós disponibilizar váriaveis e metódos que permitiram nós fazermos o inspetor ao nosso gosto, porém não disponibilizará valores referentes á alguma função da Unity, você precisará dá-los você mesmo, basicamente você estará recebendo a carcaça do Input, popup, textbox, etc., para você mostrar os valores que serão sobrescritos você precisa pega-los por uma váriavel e coloca-los como valor nos Inputs do Editor.
  

