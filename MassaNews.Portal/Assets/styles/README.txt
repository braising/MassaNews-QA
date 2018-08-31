
/// REGRAS GERAIS ///

  * Reduzir, quando possivel, a requisições de CSS por página.
  * Separar por quebra de linha seletores combinados.
  * Usar padrão de nomeclatura nas classes (descrito mais abaixo).
  * Ter no máximo 4 níveis de profundidade para os seletores (contando com as pseudo classes).
  * Quebrar arquivos de estilo muito grandes em componentes menores.
  * Evite usar ID para estilizar. Usar apenas para elementos de tagueamento, labels, manipulação com js e links internos da página.
  * Evite usar !important. Salvo as classes utilitárias.
  * Se uma pseudo classe :hover for usada, combine com a :focus para mais acessibilidade.
  * Seja generoso com os comentários, principalmente em hacks e gambiarras. Usar comentarios inline.
  * Estilos padrões de layout, como cores, fontes, bordas, paddings, etc... devem ser usados com variáveis.




/// NOMECLATURA DE CLASSES ///

  u-element     => CLASSES ULTILITÁRIAS
                   Classes com um ou mais estilos específicos. Utilizam o !important para forçar as alterações

  g-element     => CLASSES DE GRID
                   Usadas para elementos de estrutura. Trabalham com flutuações, larguras e margins horizontais

  t-element     => CLASSES DE TEMA
                   Elas não devem interferir na estrutura das páginas, apenas estilizar a aparencia de acordo com o tema do layout. Trabalham com cores, bgs, etc

  c-element     => CLASSES DE COMPONENTE
                   Módulos reaproveitaveis dentro do site. As classes sem prefixos dentro dos componentes só podem receber estilos do proprio componente ou dos arquivos da pasta base. É válido usar componentes dentro de componentes e classes modificadoras do prorpio componente para aplicar pequenas diferenças.

  is-state      => CLASSES DE ESTADO
                   Usadas para modificar o estado atual ou um comportamento do elemento.
                   Inseridas via JS. Ex: is-hidden, is-collapsed, is-active.

  content       => NOMES EM INGLÊS
                   Usar o padrão de nomeclatura das classes com termos, adjetivos, etc, em inglês. Salvo exceções que o termo seja um nome proprio ou uma sessão nomeada do site.

  main-nav      => NOMES COMPOSTOS
                   Padrão para nomear qualquer elemento com um nome composto.
                   Palavras em minusculas separadas por hífen.

  .c-header__superior => CLASSES DESCENDENTES
                   Usadas para relacionar uma hierarquia entre um elemento pai (prefixo) e seu descendente / elemento filho (sulfixo). Podem ser utilizadas para criar sub-componentes dentro de um componente, assim, não havendo necessidade de criar um novo. A classe obrigatoriamente deve estar dentro da classe principal Ex: .c-header .c-header__inner

  .main--variation => CLASSES DE MODIFICAÇÃO
                   Sobrescrever estilos especificos de uma classe já existente. Usar o seletor modificado após o seletor original, aproveitando a cascata de estilos(css). ex: .main {...} .main--variation {...}

  icon-arrow       => CLASSES DE FONT ICON
                   Font icon é uma fonte instalada para ser usada como icones. Usa prefixo icon- seguido pelo nome do icone. ex: arrow-up.png => .icon-arrow-up

  sprite-arrow  => CLASSES DE FONT ICON
                   Sprite é uma imagem só contendo varios icones. Se usa o background-position para selecionar o icone desejado. Prefixo sprite- seguido pelo nome original do icone. ex: arrow.png (antes de virar o sprite) => .sprite-arrow




/// DIRETÓRIOS BÁSICOS ///

  styles/
  |– util/
  |   |– _functions.scss   # Funções gerais/genéricas da aplicação
  |   |– _utility-class.scss # Classes utilitárias (u-)
  |
* |– base/
  |   |– _reset.scss       # Reset/normalize
  |   |– _breakpoint.scss  # Variaveis e funções de break points
  |   |– _theme.scss       # Variaveis de cores, paddings, backgrounds e demais estilizações visuais. Classes de temas (t-)
  |   |– _typography.scss  # Declarações de fonte-face, funções e estilizações de textos gerais
  |   |– _basic.scss       # Estilos basicos que afetam todo o site e estilização das demais tags básicas do html
  |   |– _grid.scss        # Clearfix, funções para grids e classes de grid (g-)
  |   |– _form.scss        # Estilos gerais dos elementos basicos de formulários
  |
  |– components/ (classes de componentes [c-])
  |   |– _.scss            #
  |   |– _.scss            #
  |   |– _.scss            #
  |   ...                  # Etc…
  |
  |– pages/ (classes personalizadas para uma página específica)
  |   |– _.scss            #
  |   |– _.scss            #
  |   |– _.scss            #
  |   ...                  # Etc…
  |
  |– vendors/
  |   |– jquery.min.css   # Jquery library
  |   |– slider.css       # slider
  |   |– jquery-ui.css    # jQuery UI
  |   ...                 # Etc…
  |
  `– application.scss      # Admin file
  `– print.scss            # Estilos para impressão
  `– vendor.scss           # Vendors admin file
  `– ie#.scss              # Hacks para versões do IE
  `– README.txt            # CSS model developer especifications


* importar na mesma ordem do esquema acima
