$(document).ready(function () {
    carregarBeneficiarios();
});

let modoAlteracao = false;
let cpfOriginal = null;

function carregarBeneficiarios() {
    $.get('/Cliente/ListarBeneficiarios', function (data) {
        renderizarTabela(data);
    });
}

function incluirBeneficiario() {
    const cpf = $('#cpfBeneficiario').val();
    const nome = $('#nomeBeneficiario').val();

    if (!cpf || !nome) {
        alert("Preencha CPF e Nome.");
        return;
    }

    //Verifica CPF Válido
    if (!ValidarCpfIncluirBeneficiarios(cpf)) {
        return;
    }

    // Verifica duplicidade no grid
    if (!modoAlteracao && cpfJaExiste(cpf)) {
        alert("Este CPF já foi adicionado à lista.");
        return;
    }

    // Se está alterando e mudou o CPF, valida se o novo CPF já existe
    if (modoAlteracao && cpf !== cpfOriginal && cpfJaExiste(cpf)) {
        alert("Já existe um beneficiário com este novo CPF.");
        return;
    }

    if (modoAlteracao) {
        $.post('/Cliente/AlterarBeneficiario', {
            CPF: cpf,
            Nome: nome,
            CPFOriginal: cpfOriginal
        }, function (data) {
            limparCampos();
            renderizarTabela(data);
        });
    } else {
        $.post('/Cliente/IncluirBeneficiario', {
            CPF: cpf,
            Nome: nome
        }, function (data) {
            limparCampos();
            renderizarTabela(data);
        });
    }
}

function ValidarCpfIncluirBeneficiarios(cpf) {
    var isvalid = false;

    $.ajax({
        url: '/Cliente/ValidarCpfBeneficiario',
        method: 'POST',
        dataType: 'JSON',
        data: { cpf: cpf },
        async: false,
        success: function (result) {
            isvalid = result;
        },
        error: function (xhr, status, error) {
            alert('Erro na Validação da CPF: ' + error);
        }
    });

    if (!isvalid) {
        alert('CPF Inválido');
        $('#cpfBeneficiario').val('');
        $('#nomeBeneficiario').val('');
    }

    return isvalid;
}

function excluirBeneficiario(cpf) {
    $.post('/Cliente/ExcluirBeneficiario', { cpf: cpf }, function (data) {
        renderizarTabela(data);
    });
}

function prepararAlteracao(cpf, nome) {
    cpfOriginal = cpf;
    $('#cpfBeneficiario').val(cpf);
    $('#nomeBeneficiario').val(nome);
    $('#btnIncluirAlterar').text('Alterar');
    modoAlteracao = true;

    $('#tabelaBeneficiarios button').prop('disabled', true);
}

function limparCampos() {
    $('#cpfBeneficiario').val('');
    $('#nomeBeneficiario').val('');
    $('#btnIncluirAlterar').text('Incluir');
    modoAlteracao = false;
    cpfOriginal = null;

    $('#tabelaBeneficiarios button').prop('disabled', false);
}

function renderizarTabela(beneficiarios) {
    const tbody = $('#tabelaBeneficiarios');
    tbody.empty();
    $.each(beneficiarios, function (index, item) {
        tbody.append(`
            <tr>
                <td>${item.CPF}</td>
                <td>${item.Nome}</td>
                <td>
                    <button class="btn btn-sm btn-primary" onclick="prepararAlteracao('${item.CPF}', '${item.Nome}')">Alterar</button>
                    <button class="btn btn-sm btn-danger" onclick="excluirBeneficiario('${item.CPF}')">Excluir</button>
                </td>
            </tr>
        `);
    });
}

function excluirBeneficiario(cpf) {
    $.post('/Cliente/ExcluirBeneficiarioSession', { cpf: cpf }, function (data) {
        renderizarTabela(data);
    });
}

function cpfJaExiste(cpf) {
    let existe = false;

    $('#tabelaBeneficiarios tr').each(function () {
        const cpfDaLinha = $(this).find('td').eq(0).text();

        if (cpfDaLinha === cpf) {
            existe = true;
        }
    });

    return existe;
}

function maskbn(o, f) {
    setTimeout(function () {
        o.value = f(o.value);
    }, 1);
}

function execmaskbn() {
    v_obj.value = v_fun(v_obj.value)
}

function maskcpfbn(v) {
    v = v.replace(/\D/g, "");
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2");
    return v;
}

function idcssbn(el) {
    return document.getElementById(el);
}

window.addEventListener('load', function () {
    let telInput = idcssbn('Telefone');
    if (telInput) {
        telInput.setAttribute('maxlength', 15);
        telInput.addEventListener('input', function () {
            maskbn(this, masktel);
        });
    }

    let cpfInput = idcssbn('cpfBeneficiario');
    if (cpfInput) {
        cpfInput.setAttribute('maxlength', 14);
        cpfInput.addEventListener('input', function () {
            maskbn(this, maskcpf);
        });
    }
});



//function carregarBeneficiarios() {
//    $.get('/Cliente/ListarBeneficiarios', function (data) {
//        renderizarTabela(data);
//    });
//}

//$(document).ready(function () {
//    carregarBeneficiarios();
//});

//let modoAlteracao = false;



//function incluirBeneficiario() {
//    const cpf = $('#cpfBeneficiario').val();
//    const nome = $('#nomeBeneficiario').val();

//    if (!cpf || !nome) {
//        alert("Preencha CPF e Nome.");
//        return;
//    }

//    if (modoAlteracao) {
//        $.post('/Cliente/AlterarBeneficiario', { CPF: cpf, Nome: nome }, function (data) {
//            limparCampos();
//            renderizarTabela(data);
//        });
//    } else {
//        $.post('/Cliente/IncluirBeneficiario', { CPF: cpf, Nome: nome }, function (data) {
//            limparCampos();
//            renderizarTabela(data);
//        });
//    }
//}

//function excluirBeneficiario(cpf) {
//    $.post('/Cliente/ExcluirBeneficiario', { cpf: cpf }, function (data) {
//        renderizarTabela(data);
//    });
//}

//function prepararAlteracao(cpf, nome) {
//    $('#cpfBeneficiario').val(cpf).prop('disabled', true);
//    $('#nomeBeneficiario').val(nome);
//    $('#btnIncluirAlterar').text('Alterar');
//    modoAlteracao = true;
//}

//function limparCampos() {
//    $('#cpfBeneficiario').val('').prop('disabled', false);
//    $('#nomeBeneficiario').val('');
//    $('#btnIncluirAlterar').text('Incluir');
//    modoAlteracao = false;
//}

//function renderizarTabela(beneficiarios) {
//    const tbody = $('#tabelaBeneficiarios');
//    tbody.empty();
//    $.each(beneficiarios, function (index, item) {
//        tbody.append(`
//            <tr>
//                <td>${item.CPF}</td>
//                <td>${item.Nome}</td>
//                <td>
//                    <button class="btn btn-sm btn-primary" onclick="prepararAlteracao('${item.CPF}', '${item.Nome}')">Alterar</button>
//                    <button class="btn btn-sm btn-danger" onclick="excluirBeneficiario('${item.CPF}')">Excluir</button>
//                </td>
//            </tr>
//        `);
//    });
//}