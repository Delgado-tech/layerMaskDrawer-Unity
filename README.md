# LayerMask no Editor [Unity] 

  Quando estamos criando um jogo na Unity é muito comum criarmos várias váriaveis de controle, e isso pode acabar fazendo com que o nosso Inspetor de objeto fique muito desorganizado e poluído, e para resolvermos isso podemos criar um script a parte extendendo á classe Editor que possíbilita montar e organizar o Inspetor ao nosso gosto. 
<br><br>
  Dentro da classe do Editor nos sobrescrevemos o metódo referente á do inspetor:
  ```cs 
  public override void OnInspectorGUI()
  ``` 
  Ela irá nós disponibilizar váriaveis e metódos que permitiram nós fazermos o inspetor ao nosso gosto, porém não disponibiliza valores referentes á alguma função da Unity, você precisa dá-los você mesmo...
  

