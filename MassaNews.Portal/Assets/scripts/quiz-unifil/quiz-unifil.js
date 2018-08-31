$(function () {
  "use strict";

    SetProgress();

    //---------------------
    // Go Back
    $(document).on("click", ".trigger-form-questions-go-back", function ()
    {

      var objQuestion = $(".form-question:visible");

      if ($(objQuestion).data("question") == 2)
        $(".go-back").hide();

      $(objQuestion).children('.selected').removeClass('selected');
      $(objQuestion).hide();
      $(objQuestion).prev().show();
      SetProgress();
    });

    // Question label
    $(".form-question").on("click", "label", function ()
    {
      
      $(".go-back").show();

      $(this).parent().children('.selected').removeClass('selected');
      $(this).addClass( "selected" );
      $(this).parent().hide();
      $(this).parent().next().show();
      SetProgress();
    });

    $(".form-optin").on("click","button",function (){
      var ok = true;

      //Get Values
      var inputnome = $(".form-optin input[name='form-nome']");
      var inputemail = $(".form-optin input[name='form-email']");
      var inputcpf = $(".form-optin input[name='form-cpf']");
      var inputnews = $(".form-optin input[name='form-news']");
      
      //Clean errors
      inputnome.next().hide();
      inputemail.next().hide();
      inputemail.next().next().hide();
      inputcpf.next().hide();

      // Nome
      if ((/^\s*$/).test(inputnome.val())) 
      {
        ok = false;
        inputnome.next().show();
      }

      // Email
      if ((/^\s*$/).test(inputemail.val())) 
      {
        ok = false;
        inputemail.next().show();
      } 
      else if (!(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/).test(inputemail.val())) 
      {
        ok = false;
        inputemail.next().next().show();
      }

      // CPF
      if (!(/^\s*$/).test(inputcpf.val()))
      {
        if (!CPFIsValid(inputcpf.val())) {
          ok = false;
          inputcpf.next().show();
        }
      }
      if (ok) {
        $.ajax({
          type: "POST",
          url: $(this).data("/quiz-unifil"),

          data:
          {
            Nome: inputnome.val(),
            Email: inputemail.val(),
            Cpf: inputcpf.val(),
            Resultado: $("input[name='form-result']").val(),
            OptinNews: $(inputnews).is(':checked'),
          },
          success: function () {
            $(".form-optin").hide();
            $(".result").show();
          },
          error: function (e) {
            alert('Ops! Algo deu errado, tente novamente.')
          }
        });
      }
    });

});

function SetProgress() {
  var nQuestions = $('.form-question').length;
  var nAnswers = $('.form-question .selected').length;
  var percent = Math.round((nAnswers * 100) / nQuestions);
  var win = '';

  $(".percent").html(percent.toString() + '%');
  $(".completed").css('width', percent.toString() + '%');
  $(".complete-questions").html(nAnswers);
  $(".total-questions").html(nQuestions);

  if (percent == 100) {
    $(".form-questions").hide();
    $(".form-optin").show();

    var opcoes = ['a', 'b', 'c', 'd', 'e'];
    var total = 0;

    $.each(opcoes, function (index, value) {
      if ($(".form-question .selected[data-answers='" + value + "']").length > total)
        win = value;
    });

    $("#result-" + win).show();

    $("input[name='form-result']").val(win);

  }
}

function CPFIsValid(strCPF) {
  strCPF = strCPF.replace('.', '').replace('.', '').replace('-', '');

  var Soma;
  var Resto;

  Soma = 0;
  if (strCPF == "00000000000") return false;

  for (i = 1; i <= 9; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
  Resto = (Soma * 10) % 11;

  if ((Resto == 10) || (Resto == 11)) Resto = 0;
  if (Resto != parseInt(strCPF.substring(9, 10))) return false;

  Soma = 0;
  for (i = 1; i <= 10; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
  Resto = (Soma * 10) % 11;

  if ((Resto == 10) || (Resto == 11)) Resto = 0;
  if (Resto != parseInt(strCPF.substring(10, 11))) return false;
  return true;
}