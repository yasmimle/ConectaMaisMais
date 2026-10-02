// ===============================
// MENU LATERAL
// ===============================

function toggleMenu(menuId) {

    const menu = document.getElementById(menuId);

    if (!menu) return;

    menu.classList.toggle("show");
}

// ===============================
// TROCAR TELAS
// ===============================

function showSection(sectionId) {

    const sections = document.querySelectorAll(".section");

    sections.forEach(sec => {
        sec.style.display = "none";
    });

    const section = document.getElementById(sectionId);

    if (section) {
        section.style.display = "block";
    }
}

// ===============================
// PERFIL
// ===============================

function toggleProfile() {

    const dropdown = document.getElementById("profileDropdown");

    if (!dropdown) return;

    dropdown.classList.toggle("hidden");
}

// ===============================
// INICIAR SISTEMA
// ===============================

document.addEventListener("DOMContentLoaded", () => {

    showSection("listarUsuarios");

    carregarUsuarios();
    carregarInteresses();
    carregarAdministradores();
    carregarAtendentes();
    configurarPesquisas();
    carregarDadosONG();
    carregarPerfil();
});
document.addEventListener("DOMContentLoaded", function () {
    const email = localStorage.getItem("adminEmail");
    const emailEl = document.getElementById("adminEmailText");

    if (emailEl && email) {
        emailEl.textContent = email;
    }

    carregarDadosONG();
});

// ===============================
// USUÁRIOS
// ===============================

function carregarUsuarios() {

    const lista = document.getElementById("listaUsuarios");

    if (!lista) return;

    lista.innerHTML = "";

    const usuarios =
        JSON.parse(localStorage.getItem("usuariosCadastrados")) || [];

    usuarios.forEach((usuario, index) => {

        const tr = document.createElement("tr");

        tr.innerHTML = `
            <td>${usuario.nome || ""}</td>
            <td>${usuario.cpf || ""}</td>
            <td>${usuario.email || ""}</td>
            <td>${usuario.telefone || ""}</td>
            <td>${usuario.endereco || ""}</td>

            <td>
                <div class="dropdown-actions">

                    <button onclick="toggleActions(this)">
                        Ações ▼
                    </button>

                    <div class="actions-menu hidden">

                        <a onclick="abrirEditarUsuario('telefone', ${index})">
                            Editar telefone
                        </a>

                        <a onclick="abrirEditarUsuario('email', ${index})">
                            Editar e-mail
                        </a>

                        <a onclick="abrirEditarUsuario('endereco', ${index})">
                            Editar endereço
                        </a>

                    </div>
                </div>
            </td>
        `;

        lista.appendChild(tr);
    });
}

// ===============================
// INTERESSES
// ===============================

function carregarInteresses() {

    const lista = document.getElementById("listaInteresses");

    if (!lista) return;

    lista.innerHTML = "";

    const interesses =
        JSON.parse(localStorage.getItem("interessesCadastrados")) || [];

    interesses.forEach((item, index) => {

        const tr = document.createElement("tr");

        tr.innerHTML = `
            <td>${item.nome || ""}</td>

            <td>
                <span class="tipo ${item.tipo || ""}">
                    ${item.tipo || ""}
                </span>
            </td>

            <td>${item.necessidade || ""}</td>

            <td>
                <span class="${item.statusClasse || "pendente"}">
                    ${item.status || "Pendente"}
                </span>
            </td>

            <td>${item.dataSolicitacao || ""}</td>

            <td>
                <div class="dropdown-actions">

                    <button onclick="toggleActions(this)">
                        Ações ▼
                    </button>

                    <div class="actions-menu hidden">

                        <a onclick="abrirEditarInteresse('status', ${index})">
                            Editar status
                        </a>

                        <a onclick="abrirEditarInteresse('descricao', ${index})">
                            Editar descrição
                        </a>

                        <a onclick="abrirEditarInteresse('tipo', ${index})">
                            Editar tipo
                        </a>

                        <a onclick="removerInteresse(${index})">
                            Remover
                        </a>

                    </div>
                </div>
            </td>
        `;

        lista.appendChild(tr);
    });
}

// ===============================
// ADMINISTRADORES
// ===============================

function carregarAdministradores() {

    const lista = document.getElementById("listaAdministradores");

    if (!lista) return;

    lista.innerHTML = "";

    const administradores =
        JSON.parse(localStorage.getItem("administradores")) || [];

    administradores.forEach((admin, index) => {

        const tr = document.createElement("tr");

        tr.innerHTML = `
            <td>${admin.nome || ""}</td>

            <td>${admin.email || ""}</td>

            <td>
                <div class="senha-box">

                    <span id="senha-admin-${index}">
                        ••••••
                    </span>

                    <button
                        class="btn-olho"
                        onclick="toggleSenha('senha-admin-${index}', '${admin.senha || ""}')"
                    >
                        <i class="fa-regular fa-eye"></i>
                    </button>

                </div>
            </td>

            <td>
                <div class="dropdown-actions">

                    <button onclick="toggleActions(this)">
                        Ações ▼
                    </button>

                    <div class="actions-menu hidden">

                        <a onclick="abrirEdicaoCadastro('admin', 'email', ${index})">
                            Editar e-mail
                        </a>

                        <a onclick="abrirEdicaoCadastro('admin', 'senha', ${index})">
                            Editar senha
                        </a>

                        <a onclick="removerCadastro('admin', ${index})">
                            Remover
                        </a>

                    </div>
                </div>
            </td>
        `;

        lista.appendChild(tr);
    });
}

// ===============================
// ATENDENTES
// ===============================

function carregarAtendentes() {

    const lista = document.getElementById("listaAtendentes");

    if (!lista) return;

    lista.innerHTML = "";

    const atendentes =
        JSON.parse(localStorage.getItem("atendentes")) || [];

    atendentes.forEach((atendente, index) => {

        const tr = document.createElement("tr");

        tr.innerHTML = `
            <td>${atendente.nome || ""}</td>

            <td>${atendente.email || ""}</td>

            <td>
                <div class="senha-box">

                    <span id="senha-atendente-${index}">
                        ••••••
                    </span>

                    <button
                        class="btn-olho"
                        onclick="toggleSenha('senha-atendente-${index}', '${atendente.senha || ""}')"
                    >
                        <i class="fa-regular fa-eye"></i>
                    </button>

                </div>
            </td>

            <td>
                <div class="dropdown-actions">

                    <button onclick="toggleActions(this)">
                        Ações ▼
                    </button>

                    <div class="actions-menu hidden">

                        <a onclick="abrirEdicaoCadastro('atendente', 'email', ${index})">
                            Editar e-mail
                        </a>

                        <a onclick="abrirEdicaoCadastro('atendente', 'senha', ${index})">
                            Editar senha
                        </a>

                        <a onclick="removerCadastro('atendente', ${index})">
                            Remover
                        </a>

                    </div>
                </div>
            </td>
        `;

        lista.appendChild(tr);
    });
}

// ===============================
// DROPDOWN AÇÕES
// ===============================

function toggleActions(button) {

    const menu =
        button.parentElement.querySelector(".actions-menu");

    if (!menu) return;

    menu.classList.toggle("hidden");
}

// ===============================
// SENHA OLHO
// ===============================

function toggleSenha(id, senhaReal) {

    const campo = document.getElementById(id);

    if (!campo) return;

    if (campo.textContent.trim() === "••••••") {

        campo.textContent = senhaReal;

    } else {

        campo.textContent = "••••••";
    }
}

function mostrarSenhaModal() {

    const input =
        document.getElementById("valorEditarCadastro");

    if (!input) return;

    if (input.type === "password") {

        input.type = "text";

    } else {

        input.type = "password";
    }
}

// ===============================
// MODAL USUÁRIO
// ===============================

let usuarioEditando = null;
let campoEditando = null;

function abrirEditarUsuario(campo, index) {

    usuarioEditando = index;
    campoEditando = campo;

    const modal =
        document.getElementById("modalEditarUsuario");

    const titulo =
        document.getElementById("tituloEditarUsuario");

    const input =
        document.getElementById("editUsuarioValor");

    const usuarios =
        JSON.parse(localStorage.getItem("usuariosCadastrados")) || [];

    const usuario = usuarios[index];

    titulo.textContent =
        `Editar ${campo}`;

    input.value =
        usuario[campo] || "";

    modal.classList.remove("hidden");
}

function fecharModalEditarUsuario() {

    document
        .getElementById("modalEditarUsuario")
        .classList.add("hidden");
}

function salvarEdicaoUsuario() {

    const usuarios =
        JSON.parse(localStorage.getItem("usuariosCadastrados")) || [];

    const valor =
        document.getElementById("editUsuarioValor").value;

    usuarios[usuarioEditando][campoEditando] = valor;

    localStorage.setItem(
        "usuariosCadastrados",
        JSON.stringify(usuarios)
    );

    fecharModalEditarUsuario();

    carregarUsuarios();

    abrirSucesso();
}

// ===============================
// MODAL INTERESSE
// ===============================

let interesseEditando = null;
let campoInteresse = null;

function abrirEditarInteresse(campo, index) {

    interesseEditando = index;
    campoInteresse = campo;

    const modal =
        document.getElementById("modalEditarInteresse");

    const titulo =
        document.getElementById("tituloEditarInteresse");

    const conteudo =
        document.getElementById("conteudoEditarInteresse");

    const interesses =
        JSON.parse(localStorage.getItem("interessesCadastrados")) || [];

    const item = interesses[index];

    titulo.textContent = `Editar ${campo}`;

    conteudo.innerHTML = `
        <input
            type="text"
            id="novoValorInteresse"
            value="${item[campo] || ""}"
        >
    `;

    modal.classList.remove("hidden");
}

function fecharModalEditarInteresse() {

    document
        .getElementById("modalEditarInteresse")
        .classList.add("hidden");
}

function salvarEdicaoInteresse() {

    const interesses =
        JSON.parse(localStorage.getItem("interessesCadastrados")) || [];

    const valor =
        document.getElementById("novoValorInteresse").value;

    interesses[interesseEditando][campoInteresse] = valor;

    localStorage.setItem(
        "interessesCadastrados",
        JSON.stringify(interesses)
    );

    fecharModalEditarInteresse();

    carregarInteresses();

    abrirSucesso();
}

function removerInteresse(index) {

    const interesses =
        JSON.parse(localStorage.getItem("interessesCadastrados")) || [];

    interesses.splice(index, 1);

    localStorage.setItem(
        "interessesCadastrados",
        JSON.stringify(interesses)
    );

    carregarInteresses();
}

// ===============================
// ADMIN / ATENDENTE
// ===============================

let tipoCadastro = null;
let cadastroIndex = null;
let campoCadastro = null;

function abrirEdicaoCadastro(tipo, campo, index) {

    tipoCadastro = tipo;
    cadastroIndex = index;
    campoCadastro = campo;

    const modal =
        document.getElementById("modalEditarCadastro");

    const titulo =
        document.getElementById("tituloEditarCadastro");

    const input =
        document.getElementById("valorEditarCadastro");

    const lista =
        JSON.parse(localStorage.getItem(
            tipo === "admin"
                ? "administradores"
                : "atendentes"
        )) || [];

    const item = lista[index];

    if (campo === "email") {

        titulo.textContent = "Editar e-mail";

        input.placeholder = "Novo e-mail";

        input.value = item.email || "";

        input.type = "email";
    }

    if (campo === "senha") {

        titulo.textContent = "Editar senha";

        input.placeholder = "Nova senha";

        input.value = item.senha || "";

        input.type = "password";
    }

    modal.classList.remove("hidden");
}

function fecharModalEditarCadastro() {

    document
        .getElementById("modalEditarCadastro")
        .classList.add("hidden");
}

function salvarEdicaoCadastro() {

    const chave =
        tipoCadastro === "admin"
            ? "administradores"
            : "atendentes";

    const lista =
        JSON.parse(localStorage.getItem(chave)) || [];

    lista[cadastroIndex][campoCadastro] =
        document.getElementById("valorEditarCadastro").value;

    localStorage.setItem(
        chave,
        JSON.stringify(lista)
    );

    fecharModalEditarCadastro();

    carregarAdministradores();

    carregarAtendentes();

    abrirSucesso();
}

function removerCadastro(tipo, index) {

    const chave =
        tipo === "admin"
            ? "administradores"
            : "atendentes";

    const lista =
        JSON.parse(localStorage.getItem(chave)) || [];

    lista.splice(index, 1);

    localStorage.setItem(
        chave,
        JSON.stringify(lista)
    );

    carregarAdministradores();

    carregarAtendentes();
}

// ===============================
// SUCESSO
// ===============================

function abrirSucesso() {

    document
        .getElementById("sucessoModal")
        .classList.remove("hidden");
}

function fecharSucesso() {

    document
        .getElementById("sucessoModal")
        .classList.add("hidden");
}

// ===============================
// PESQUISA
// ===============================

function configurarPesquisas() {

    pesquisar(
        "searchUsuarios",
        "listaUsuarios"
    );

    pesquisar(
        "searchInteresses",
        "listaInteresses"
    );

    pesquisar(
        "searchAdministradores",
        "listaAdministradores"
    );

    pesquisar(
        "searchAtendentes",
        "listaAtendentes"
    );
}

function pesquisar(inputId, tabelaId) {

    const input =
        document.getElementById(inputId);

    const tabela =
        document.getElementById(tabelaId);

    if (!input || !tabela) return;

    input.addEventListener("keyup", () => {

        const valor =
            input.value.toLowerCase();

        const linhas =
            tabela.querySelectorAll("tr");

        linhas.forEach(linha => {

            const texto =
                linha.textContent.toLowerCase();

            linha.style.display =
                texto.includes(valor)
                    ? ""
                    : "none";
        });
    });
}
// ===============================
// CRIAR ADMIN
// ===============================

function abrirCriarAdmin() {

    const modal =
        document.getElementById("modalCriarUsuario");

    const titulo =
        document.getElementById("tituloCriarUsuario");

    titulo.textContent = "Criar Administrador";

    modal.classList.remove("hidden");

    modal.dataset.tipo = "admin";
}

// ===============================
// CRIAR ATENDENTE
// ===============================

function abrirCriarAtendente() {

    const modal =
        document.getElementById("modalCriarUsuario");

    const titulo =
        document.getElementById("tituloCriarUsuario");

    titulo.textContent = "Criar Atendente";

    modal.classList.remove("hidden");

    modal.dataset.tipo = "atendente";
}

// ===============================
// FECHAR MODAL
// ===============================

function fecharModalCriarUsuario() {

    document
        .getElementById("modalCriarUsuario")
        .classList.add("hidden");
}

// ===============================
// SALVAR NOVO USUÁRIO
// ===============================

function salvarNovoUsuario() {

    const nome =
        document.getElementById("novoNomeUsuario").value;

    const email =
        document.getElementById("novoEmailUsuario").value;

    const senha =
        document.getElementById("novaSenhaUsuario").value;

    const modal =
        document.getElementById("modalCriarUsuario");

    const tipo =
        modal.dataset.tipo;

    const chave =
        tipo === "admin"
            ? "administradores"
            : "atendentes";

    const lista =
        JSON.parse(localStorage.getItem(chave)) || [];

    lista.push({
        nome,
        email,
        senha
    });

    localStorage.setItem(
        chave,
        JSON.stringify(lista)
    );

    fecharModalCriarUsuario();

    carregarAdministradores();

    carregarAtendentes();

    abrirSucesso();
}
// ===============================
// DADOS ONG
// ===============================

function editarONG(campo) {
    const campos = {
        telefone: {
            texto: "ongTelefoneText",
            input: "ongTelefoneInput"
        },
        email: {
            texto: "ongEmailText",
            input: "ongEmailInput"
        },
        endereco: {
            texto: "ongEnderecoText",
            input: "ongEnderecoInput"
        }
    };

    const alvo = campos[campo];
    if (!alvo) return;

    const span = document.getElementById(alvo.texto);
    const input = document.getElementById(alvo.input);

    if (!span || !input) return;

    span.classList.add("hidden-input");
    input.classList.remove("hidden-input");
    input.focus();
}

function salvarDadosONG() {
    const telefone = document.getElementById("ongTelefoneInput").value.trim();
    const email = document.getElementById("ongEmailInput").value.trim();
    const endereco = document.getElementById("ongEnderecoInput").value.trim();

    localStorage.setItem("ongTelefone", telefone);
    localStorage.setItem("ongEmail", email);
    localStorage.setItem("ongEndereco", endereco);

    document.getElementById("ongTelefoneText").textContent = telefone;
    document.getElementById("ongEmailText").textContent = email;
    document.getElementById("ongEnderecoText").textContent = endereco;

    document.getElementById("ongTelefoneText").classList.remove("hidden-input");
    document.getElementById("ongEmailText").classList.remove("hidden-input");
    document.getElementById("ongEnderecoText").classList.remove("hidden-input");

    document.getElementById("ongTelefoneInput").classList.add("hidden-input");
    document.getElementById("ongEmailInput").classList.add("hidden-input");
    document.getElementById("ongEnderecoInput").classList.add("hidden-input");

    alert("Dados da ONG salvos com sucesso!");
}

function carregarDadosONG() {
    const telefone = localStorage.getItem("ongTelefone");
    const email = localStorage.getItem("ongEmail");
    const endereco = localStorage.getItem("ongEndereco");

    if (telefone) {
        document.getElementById("ongTelefoneText").textContent = telefone;
        document.getElementById("ongTelefoneInput").value = telefone;
    }

    if (email) {
        document.getElementById("ongEmailText").textContent = email;
        document.getElementById("ongEmailInput").value = email;
    }

    if (endereco) {
        document.getElementById("ongEnderecoText").textContent = endereco;
        document.getElementById("ongEnderecoInput").value = endereco;
    }
}
function editarONG(campo) {
    const campos = {
        telefone: {
            texto: "ongTelefoneText",
            input: "ongTelefoneInput"
        },
        email: {
            texto: "ongEmailText",
            input: "ongEmailInput"
        },
        endereco: {
            texto: "ongEnderecoText",
            input: "ongEnderecoInput"
        }
    };

    const alvo = campos[campo];
    if (!alvo) return;

    const span = document.getElementById(alvo.texto);
    const input = document.getElementById(alvo.input);

    if (!span || !input) return;

    span.classList.add("hidden-input");
    input.classList.remove("hidden-input");
    input.focus();
}

function salvarDadosONG() {
    const telefone = document.getElementById("ongTelefoneInput").value.trim();
    const email = document.getElementById("ongEmailInput").value.trim();
    const endereco = document.getElementById("ongEnderecoInput").value.trim();

    localStorage.setItem("ongTelefone", telefone);
    localStorage.setItem("ongEmail", email);
    localStorage.setItem("ongEndereco", endereco);

    document.getElementById("ongTelefoneText").textContent = telefone;
    document.getElementById("ongEmailText").textContent = email;
    document.getElementById("ongEnderecoText").textContent = endereco;

    document.getElementById("ongTelefoneText").classList.remove("hidden-input");
    document.getElementById("ongEmailText").classList.remove("hidden-input");
    document.getElementById("ongEnderecoText").classList.remove("hidden-input");

    document.getElementById("ongTelefoneInput").classList.add("hidden-input");
    document.getElementById("ongEmailInput").classList.add("hidden-input");
    document.getElementById("ongEnderecoInput").classList.add("hidden-input");

    alert("Dados da ONG salvos com sucesso!");
}

function carregarDadosONG() {
    const telefone = localStorage.getItem("ongTelefone");
    const email = localStorage.getItem("ongEmail");
    const endereco = localStorage.getItem("ongEndereco");

    if (telefone) {
        document.getElementById("ongTelefoneText").textContent = telefone;
        document.getElementById("ongTelefoneInput").value = telefone;
    }

    if (email) {
        document.getElementById("ongEmailText").textContent = email;
        document.getElementById("ongEmailInput").value = email;
    }

    if (endereco) {
        document.getElementById("ongEnderecoText").textContent = endereco;
        document.getElementById("ongEnderecoInput").value = endereco;
    }
}
let campoONGEditando = "";

function abrirEdicaoONG(campo) {
    campoONGEditando = campo;

    const titulos = {
        telefone: "Editar telefone",
        email: "Editar email",
        endereco: "Editar endereço"
    };

    const valores = {
        telefone: document.getElementById("ongTelefoneText").textContent,
        email: document.getElementById("ongEmailText").textContent,
        endereco: document.getElementById("ongEnderecoText").textContent
    };

    document.getElementById("tituloEditarONG").textContent = titulos[campo];
    document.getElementById("valorEditarONG").value = valores[campo];

    document.getElementById("modalEditarONG").classList.remove("hidden");
}

function fecharEdicaoONG() {
    document.getElementById("modalEditarONG").classList.add("hidden");
}

function confirmarEdicaoONG() {
    const valor = document.getElementById("valorEditarONG").value.trim();

    if (!valor) {
        alert("Preencha o campo.");
        return;
    }

    if (campoONGEditando === "telefone") {
        document.getElementById("ongTelefoneText").textContent = valor;
        localStorage.setItem("ongTelefone", valor);
    }

    if (campoONGEditando === "email") {
        document.getElementById("ongEmailText").textContent = valor;
        localStorage.setItem("ongEmail", valor);
    }

    if (campoONGEditando === "endereco") {
        document.getElementById("ongEnderecoText").textContent = valor;
        localStorage.setItem("ongEndereco", valor);
    }

    fecharEdicaoONG();
    alert("Salvo com sucesso!");
}

function salvarDadosONG() {
    alert("Dados da ONG salvos com sucesso!");
}
let campoPerfilEditando = "";

function editarPerfil(campo) {
    campoPerfilEditando = campo;

    const titulos = {
        nome: "Editar nome",
        email: "Editar email",
        celular: "Editar celular"
    };

    const spans = {
        nome: "nomeText",
        email: "emailText",
        celular: "celularText"
    };

    const inputs = {
        nome: "nomeInput",
        email: "emailInput",
        celular: "celularInput"
    };

    const modal = document.getElementById("modalEditarPerfil");
    const titulo = document.getElementById("tituloEditarPerfil");
    const input = document.getElementById("valorEditarPerfil");

    if (!modal || !titulo || !input) return;

    titulo.textContent = titulos[campo];
    input.value = document.getElementById(spans[campo]).textContent.trim();
    input.placeholder = titulos[campo];

    modal.classList.remove("hidden");
}

function fecharEditarPerfil() {
    document.getElementById("modalEditarPerfil").classList.add("hidden");
}

function salvarEditarPerfil() {
    const valor = document.getElementById("valorEditarPerfil").value.trim();

    if (!valor) {
        alert("Preencha o campo.");
        return;
    }

    if (campoPerfilEditando === "nome") {
        document.getElementById("nomeText").textContent = valor;
        localStorage.setItem("adminNome", valor);
    }

    if (campoPerfilEditando === "email") {
        document.getElementById("emailText").textContent = valor;
        localStorage.setItem("adminEmail", valor);
    }

    if (campoPerfilEditando === "celular") {
        document.getElementById("celularText").textContent = valor;
        localStorage.setItem("adminCelular", valor);
    }

    document.getElementById("modalEditarPerfil").classList.add("hidden");
    alert("Salvo com sucesso!");
}

function carregarPerfil() {
    const nome = localStorage.getItem("adminNome");
    const email = localStorage.getItem("adminEmail");
    const celular = localStorage.getItem("adminCelular");

    if (nome) document.getElementById("nomeText").textContent = nome;
    if (email) document.getElementById("emailText").textContent = email;
    if (celular) document.getElementById("celularText").textContent = celular;
}
function sairPerfil() {
    window.location.href = "index.html";
}