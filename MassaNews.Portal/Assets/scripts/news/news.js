//Scope Values
var LocalScope =
{
  // ScopeId: '@ViewBag.ScopeId',
  // NavegationType: '@ViewBag.NavigationType',
  // AllowComments: '@ViewBag.AllowComments'

  ScopeId: $("meta[name='scope-id']").attr("content"),
  NavegationType: $("meta[name='navegation-type']").attr("content"),
  AllowComments: $("meta[name='allow-comments']").attr("content")
};

//Adiciona um novo item de navegação
function LoadNexNavegationLink()
{
  var firstid = $(".li-lastnew").first().attr("id");
  var lastid = $(".li-lastnew").last().attr("id");
  var lastdata = $(".li-lastnew").last().find('.post-data-publicacao').val();

  if (InfinityScroll.LastNewId === lastid)
    return false;

  InfinityScroll.LastNewId = lastid;

  var url = "/Noticia/LoadNexNavegationLink";

  $("<div>").load(url, { firstId: firstid, scopeId: LocalScope.ScopeId, navegationType: LocalScope.NavegationType, publishDate: lastdata }, function ()
  {
    if ($(this).find('li').length > 0)
    {
      $(".c-fixed-sidebar__list").append($(this));
      InfinityScroll.PostToLoad++;
    }
  });
}

//Adiciona um novo conteudo
function LoadNexNews(url, isFirst)
{
  var method = "/Noticia/LoadNexNews";

  $("<div>").load(method, { noticiaUrl: url, allowComments: LocalScope.AllowComments == 'True' }, function ()
  {
    if ($(this) != null)
    {
      //Recupera o post
      var objPost = $(this).find("article").first();

      //Remove o conteúdo do primeiro banner
      $(objPost).find('.c-publicidade').html('');

      //Insere o novo post
      if (isFirst){
        $(objPost).insertBefore($(".g-content").find(".c-post").first());
      } else {
        $(objPost).insertAfter($(".g-content").find(".c-post").last());
      }

      //Ajusta a galeria
      $(objPost).find(".trigger-lightgallery").each(function (index, obj){
        SetLightGallery($(obj));
      });

      //Ajuste do accordion
      SetFirstItemOpen($(objPost));

      //Adiciona publicidade
      //LoadNewAds($(objPost));

      //Disqus Count
      //DISQUSWIDGETS.getCount({ reset: true });

      //Incrementa o contador de itens carregados
      InfinityScroll.Loaded++;

      // YOUTUBE
      var newId = $(".is-current").first().attr("id");

      var newIdModif = newId.replace("li-", "");

      var div = document.getElementById('div' + newIdModif);

      if (div != null)
      {
        loadApiYT();

        var o_player = new YT.Player('player' + newIdModif);
        
        //subscribe to events
        o_player.addEventListener("onReady", "onYouTubePlayerReady");
        o_player.addEventListener("onStateChange", "onYouTubePlayerStateChange");
      }

      //Ajusta a posição da navegação
      if (isFirst)
      {
        setPostNavegation(null, 0, true);
      }
    }
  });
}

//News access notification event
function NotifyAccessedNews(newId)
{
  var method = "/Noticia/NewsAccessedEvent";

  if (newId == null)
    newId = $(".li-lastnew").first().attr("id");

  $.get(method, { newsId: newId }, function ()
  {
     //console.log('NewsAccessedEvent');
  });

}

//Event Load
$(document).ready(function (){
  //Infinityscroll Config
  InfinityScroll.Loaded = $(".c-post").length;
  InfinityScroll.PostToLoad = $(".li-lastnew").length;
  //InfinityScroll.PostPull = InfinityScroll.Loaded < 4 ? 1 : 3;
  InfinityScroll.LoadNavegationLink = LoadNexNavegationLink;
  InfinityScroll.LoadNewContent = LoadNexNews;
  InfinityScroll.NotifyAccess = NotifyAccessedNews;

  //Carrega Google ADS
  //$(".c-post").each(function (index, obj) { if (index > 0) { LoadNewAds(obj); } });
  $(".c-post").each(function (index, obj) {
    if (index > 0) {
      LoadNewAds(obj);
    }
  });

  //News access notification event
  NotifyAccessedNews(null);
});

//Holds a reference to the YouTube player
var ik_player;

//this function is called by the API
function onYouTubeIframeAPIReady() {
  //creates the player object
  //var newsId = "@Html.Raw(Model.News.News.Id)";
  var newsId = $("meta[name='news-id']").attr("content");

  var div = document.getElementById('div' + newsId);

  if (div != null) {

    ik_player = new YT.Player('player' + newsId);

    //subscribe to events
    ik_player.addEventListener("onReady", "onYouTubePlayerReady");
    ik_player.addEventListener("onStateChange", "onYouTubePlayerStateChange");
  }
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