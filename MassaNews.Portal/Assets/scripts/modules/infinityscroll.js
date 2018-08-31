//Configuração Inicial
var InfinityScroll =
{
  Loaded: 4,
  PostToLoad: 0,
  PostPull: 3,
  PostIndex: 0,
  LastUrl: "",
  LastNewId: 0,
  LoadNavegationLink: null,
  LoadNewContent: null,
  NotifyAccess: null
};

//Adiciona classe no primeiro post (ativo)
$('body').find('.c-post').first().addClass('c-post--active');

//Captura o evento de scroll
$(window).scroll(function ()
{
  var meia = ($(window).height() / 4);

  //Verifica se é necessário inserir um novo post
  verifyNeedLoadNewPost();

  //Verifica qual é post corrente
  $(".g-content").find(".c-post").each(function (index)
  {
    //Inicio
    var artOffsetTop = $(this).offset().top;

    //Fim
    var artOffsetBottom = $(this).offset().top + $(this).height();

    if (($(window).scrollTop() >= artOffsetTop - meia) && ($(window).scrollTop() <= artOffsetBottom - meia))
    {
      setPostNavegation($(this), index, false);
    }
  });
});

//Verifica se é necessário carregar mais um post
function verifyNeedLoadNewPost()
{
  if (((InfinityScroll.PostToLoad - InfinityScroll.Loaded) > 0 && (InfinityScroll.Loaded - (InfinityScroll.PostIndex + 1) - InfinityScroll.PostPull) < 0))
  {
    if (loadNewPost(null, false) && InfinityScroll.LoadNavegationLink !== null)
    {
      InfinityScroll.LoadNavegationLink();
    }
  }
}

//Efetua o carregamento de um novo post
function loadNewPost(url, isFirst)
{
  if (url === null)
    url = $(".c-fixed-sidebar__list__iten").eq(InfinityScroll.Loaded).find("a").attr("href");

  if (InfinityScroll.LastUrl === url)
    return false;

  InfinityScroll.LastUrl = url;

  if (InfinityScroll.LoadNewContent !== null)
  {
    InfinityScroll.LoadNewContent(url, isFirst);
  } else {
    $("<div>").load(url + " .c-post", function () {
      //Recupera o post
      var objPost = $(this).find("article").first();

      //insere o novo post
      if (isFirst){
        objPost.insertBefore($(".g-content").find(".c-post").first());
      } else {
        objPost.insertAfter($(".g-content").find(".c-post").last());
      }

      //Ajusta a galeria
      objPost.find(".trigger-lightgallery").each(function (index, obj) {
        SetLightGallery($(obj));
      });

      //Ajuste do accordion
      SetFirstItemOpen(objPost);

      //Adiciona publicidade
      //LoadNewAds(objPost);

      //Incrementa o contador de itens carregados
      InfinityScroll.Loaded++;

    });
  }

  return true;
}

//Seta a navegação do post
function setPostNavegation(objPost, index, isClick){
  if (objPost === null)
    objPost = $(".c-post").first();

  var url   = objPost.find(".url-post").val();
  var city  = " " + objPost.find(".trigger-get-city").text();
  var title = objPost.find("h1").first().text() + " - Massa News" + city;

  if (InfinityScroll.PostIndex != index || isClick){
    InfinityScroll.PostIndex = index;

    //Remove o css de corrente da sidebar
    $(".trigger-sidebar-ultimas").find(".is-current").removeClass("is-current");

    //Adiciona o css de corrente
    $(".trigger-sidebar-ultimas").find("li").eq(index).addClass("is-current");

    //Ajusta a posição do nano scroll
    var objLi = $(".trigger-sidebar-ultimas").find("li").eq(index).get(0);

    if(objLi !== null)
      $(".nano-content").animate({ 'scrollTop': $(".trigger-sidebar-ultimas").find("li").eq(index).get(0).offsetTop }, "slow");

    //Adiciona classe no post ativo
    $('body').find('.c-post--active').addClass('c-post--loaded');
    $('body').find('.c-post').removeClass('c-post--active');
    objPost.addClass('c-post--active');

    //Efetua a alteração da URL
    ChangePageState(title, url);

    //Carrega DFP
    LoadNewAds($(objPost));

    //Notify access
    if (InfinityScroll.NotifyAccess != null)
      InfinityScroll.NotifyAccess($(objLi).attr("id"));
  }
}

//Efetua a alteração da URL
function ChangePageState(title, url)
{
  if (typeof (history.pushState) != "undefined") {

    var stateData;
    {
      Title = title;
      Url = url;
    }

    //Atualiza o state atual com a nova url
    history.replaceState(stateData, title, url);

    //Altera o título da página
    document.title = title;


    // FACEBOOK COMMENTS
    if(!$('body').find('.c-post--active').hasClass('c-post--loaded')){
      // Box
      var commentsBox = '<div class="fb-comments" data-href="' + document.location.origin + url + '" data-width="100%" data-mobile="true" data-order-by="social" data-numposts="5"></div>';
      $(".c-post--active").find('.trigger-fb-comments').html(commentsBox);

      FB.XFBML.parse($(".c-post--active").find('.trigger-fb-comments').get(0),function(){
        $(".FB_Loader").remove();
      });

      // Count
      //var commentsCount = '<span class="fb-comments-count c-share__count" data-href="' + document.location.origin + url + '"></span>';
      //$(".c-post--active").find('.trigger-comments-count').html(commentsCount);
      // FB.XFBML.parse($(".c-post--active").find('.fb-comments-count').get(0),function(){
      //   $(".FB_Loader").remove();
      // });
    }

    //Faz um push no google analytics para contabilizar o acesso
    if(typeof dataLayer !== 'undefined') {
      dataLayer.push({
        'event': 'virtual-pageview-' + currentCityMicroregiaoUrl,
        'virtualPageURL': url,
        'virtualPageTitle': title
      });
    }
  }
}

//Efetua as regras de mudança da página
function LoadNewPage(obj)
{
  var liId = $(obj).attr("id");
  var objLi = $("#" + liId);
  var url = objLi.find("a").attr("href");

  //Remove o elemento is-hover
  objLi.find(".c-headline").removeClass("is-hover");

  $(".c-fixed-sidebar__list").prepend(objLi);

  var isLoaded = false;

  //Verefica se o post foi carregado
  $(".url-post").each(function (index)
  {
    if ($(this).val() == url)
    {
      isLoaded = true;

      if (index !== 0)
      {
        var objPost = $(".c-post").eq(index);

        $(".g-content").prepend(objPost);

      }
    }
  });
  
  //Caso o post não tenha sido carregado
  if (!isLoaded)
    loadNewPost(url, true);   

  $(document).scrollTop(0);

  LoadYT();
    
}

function LoadYT() {

  // YOUTUBE
  var newId = $(".is-current").first().attr("id");

  var newIdModif = newId.replace("li-", "");

  var div = document.getElementById('div' + newIdModif);

  if (div != null) {

    loadApiYT();

    var o_player = new YT.Player('player' + newIdModif);
    //subscribe to events
    o_player.addEventListener("onReady", "onYouTubePlayerReady");
    o_player.addEventListener("onStateChange", "onYouTubePlayerStateChange");
  }

  function onYouTubePlayerReady() {
    //closeRelatedVideos();
  }

  function onYouTubePlayerStateChange(event) {
    if (event.data === 0) {
      openRelatedVideos();
    }
  }

  function openRelatedVideos() {
    $('.c-post--active').find('.c-post__videos-related').fadeIn('fast');
    event.preventDefault();
  }

  function closeRelatedVideos() {
    $('.c-post--active').find('.c-post__videos-related').fadeOut('fast');
    event.preventDefault();
  }

  function loadApiYT() {
    var tag = document.createElement('script');

    tag.src = "https://www.youtube.com/iframe_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
  }

}