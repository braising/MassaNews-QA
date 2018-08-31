// // Author: Dirceu Wilian de Camargo Conte
// // Description: Implementação para dar suporte ao disqus

// var disqus_identifier;
// var disqus_url;

// function loadDisqus(source, identifier, url) {
//   var objPost = source.parents('.c-post').first();
//   var commentIsActive = source.parents('.c-post').hasClass('comment-active');
//   var objComments = document.querySelector('.c-comments-box');

//   // Não recarrega se o comentário já foi aberto 
//   if(!commentIsActive){
//     if (window.DISQUS) {
//       DISQUS.reset({
//         reload: true,
//         config: function () {
//           this.page.identifier = identifier;
//           this.page.url = url;
//         }
//       });
//     } else {
//       //set the identifier argument
//       disqus_identifier = identifier;

//       //set the permalink argument
//       disqus_url = url;

//       //Append the Disqus embed script to HTML
//       var dsq = document.createElement('script'), doc = document;
//       dsq.type = 'text/javascript';
//       dsq.async = true;
//       dsq.src = 'https://massanews.disqus.com/embed.js';
//       dsq.setAttribute('data-timestamp', +new Date());
//       (document.head || document.body).appendChild(dsq);
//     }

//     // Adiciona/remove a classe de comentário ativo
//     $('body').find('.comment-active').removeClass('comment-active');
//     objPost.addClass('comment-active');
//   }

//   // Abre o modal de comentários
//   objComments.classList.add('c-comments-box--open');
// }