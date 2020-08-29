function AjaxModal() {
    $(document).ready(function () {
        $(function () {
            $.ajaxSetup({ cache: false });

            $("a[data-modal]").on("click",
                function (e) {
                    $('#myModalContent').load(this.href,
                        function () {
                            $('#myModal').modal({
                                keyboard: true
                            },
                                'show');
                            bindForm(this);
                        });
                    return false;
                });
        });


        function bindForm(dialog) {
            $('form', dialog).submit(function () {
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.success) {
                            $('#myModal').modal('hide');
                            $('#EnderecoTarget').load(result.url);
                        } else {
                            $('#myModalContent').html(result);
                            bindForm(dialog);
                        }
                    }
                });
                return false;
            });
        }
    });
}

function BuscaCep() {
    $(document).ready(function () {
        function limpa_formulario_cep() {
            //Limpa os valores do formulário de cep.
            $("#Endereco_Logradouro").val("");
            $("#Endereco_Bairro").val("");
            $("#Endereco_Cidade").val("");
            $("#Endereco_Estado").val("");
        }

        //Quando o campo cep perde o foco.
        $("#Endereco_Cep").blur(function () {
            //Nova variável "cep" somente com dígitos.
            var cep = $(this).val().replace(/\D/g, '');

            if (cep != "") {
                var validacep = /^[0-9]{8}$/;

                if (validacep.test(cep)) {
                    //Preenche os campos com "..." enquanto consulta webservice
                    $("#Endereco_Logradouro").val("...");
                    $("#Endereco_Bairro").val("...");
                    $("#Endereco_Cidade").val("...");
                    $("#Endereco_Estado").val("...");

                    $.getJSON("https://viacep.com.br/" + cep + "/json/?callback=?",
                        function (dados) {
                            if (!("erro" in dados)) {
                                //Atualiza os campos com os valoes da consulta.
                                $("#Endereco_Logradouro").val(dados.logradouro);
                                $("#Endereco_Bairro").val(dados.bairro);
                                $("#Endereco_Cidade").val(dados.localidade);
                                $("#Endereco_Estado").val(dados.uf);
                            }
                            else {
                                //CEP pesquisado não foi encontrado.
                                limpa_formulario_cep();
                                alert("CEP não encontrado.");
                            }
                        });

                }
                else {
                    //cep é inválido
                    limpa_formulario_cep();
                    alert("Formato de CEP inválido.");
                }
            }
            else {
                //cep sem valor, limpar formulário.
                limpa_formulario_cep();
            }
        });

    });
}