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

let usuariosBanco = [];
let interessesBanco = [];
let administradoresBanco = [];
let atendentesBanco = [];

function textoTipoInteresse(tipo) {
    const valor = (tipo || "").toLowerCase();

    if (valor === "doar" || valor === "doador") return "Doador";
    if (valor === "receber" || valor === "recebedor" || valor === "receptor") return "Recebedor";
    if (valor === "ambos") return "Ambos";

    return "Ambos";
}

function classeTipoInteresse(tipo) {
    const valor = textoTipoInteresse(tipo).toLowerCase();

    if (valor === "doador") return "doar";
    if (valor === "recebedor") return "receber";

    return "ambos";
}

async function carregarUsuarios() {

    const lista = document.getElementById("listaUsuarios");

    if (!lista) return;

    lista.innerHTML = "";

    try {
        const resposta = await fetch("/api/admin/usuarios");

        if (!resposta.ok) {
            throw new Error("Erro ao carregar usuarios");
        }

        usuariosBanco = await resposta.json();
    } catch (erro) {
        console.error(erro);
        alert("Erro ao carregar usuários do banco.");
        return;
    }

    usuariosBanco.forEach((usuario, index) => {

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

                        <a onclick="removerUsuario(${usuario.id})">
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
// INTERESSES
// ===============================

async function carregarInteresses() {

    const lista = document.getElementById("listaInteresses");

    if (!lista) return;

    lista.innerHTML = "";

    try {
        const resposta = await fetch("/api/admin/interesses");

        if (!resposta.ok) {
            throw new Error("Erro ao carregar interesses");
        }

        interessesBanco = await resposta.json();
    } catch (erro) {
        console.error(erro);
        alert("Erro ao carregar interesses do banco.");
        return;
    }

    interessesBanco.forEach((item, index) => {

        const tr = document.createElement("tr");

        tr.innerHTML = `
            <td>${item.nome || ""}</td>

            <td>
                <span class="tipo ${classeTipoInteresse(item.tipo)}">
                    ${textoTipoInteresse(item.tipo)}
                </span>
            </td>

            <td>${item.descricao || ""}</td>

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

                        <a onclick="removerInteresse(${item.id})">
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

async function carregarAdministradores() {

    const lista = document.getElementById("listaAdministradores");

    if (!lista) return;

    lista.innerHTML = "";

    try {
        const resposta = await fetch("/api/admin/administradores");

        if (!resposta.ok) {
            throw new Error("Erro ao carregar administradores");
        }

        administradoresBanco = await resposta.json();
    } catch (erro) {
        console.error(erro);
        alert("Erro ao carregar administradores do banco.");
        return;
    }

    administradoresBanco.forEach((admin, index) => {

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

                        <a onclick="removerCadastro('admin', ${admin.id})">
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

async function carregarAtendentes() {

    const lista = document.getElementById("listaAtendentes");

    if (!lista) return;

    lista.innerHTML = "";

    try {
        const resposta = await fetch("/api/admin/atendentes");

        if (!resposta.ok) {
            throw new Error("Erro ao carregar atendentes");
        }

        atendentesBanco = await resposta.json();
    } catch (erro) {
        console.error(erro);
        alert("Erro ao carregar atendentes do banco.");
        return;
    }

    atendentesBanco.forEach((atendente, index) => {

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
                <span class="ativo">${atendente.status || "Ativo"}</span>
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

                        <a onclick="removerCadastro('atendente', ${atendente.id})">
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

    const usuario = usuariosBanco[index];

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

async function salvarEdicaoUsuario() {

    const valor =
        document.getElementById("editUsuarioValor").value;

    const usuario = usuariosBanco[usuarioEditando];

    const resposta = await fetch(`/api/admin/usuarios/${usuario.id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            campo: campoEditando,
            valor
        })
    });

    if (!resposta.ok) {
        const erro = await resposta.json().catch(() => null);
        alert(erro?.mensagem || "Erro ao editar usuário.");
        return;
    }

    fecharModalEditarUsuario();

    await carregarUsuarios();

    abrirSucesso();
}

async function removerUsuario(id) {

    if (!confirm("Deseja remover este usuário?")) return;

    const resposta = await fetch(`/api/admin/usuarios/${id}`, {
        method: "DELETE"
    });

    if (!resposta.ok) {
        const erro = await resposta.json().catch(() => null);
        alert(erro?.mensagem || "Erro ao remover usuário.");
        return;
    }

    await carregarUsuarios();
    await carregarInteresses();
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

    const item = interessesBanco[index];

    titulo.textContent = `Editar ${campo}`;

    if (campo === "status") {
        conteudo.innerHTML = `
            <select id="novoValorInteresse">
                <option value="Pendente" ${item.status === "Pendente" ? "selected" : ""}>Pendente</option>
                <option value="Concluído" ${(item.status || "").toLowerCase().includes("conclu") ? "selected" : ""}>Concluído</option>
                <option value="Cancelado" ${(item.status || "").toLowerCase() === "cancelado" ? "selected" : ""}>Cancelado</option>
            </select>
        `;
    } else {
        conteudo.innerHTML = `
            <input
                type="text"
                id="novoValorInteresse"
                value="${item[campo] || ""}"
            >
        `;
    }

    modal.classList.remove("hidden");
}

function fecharModalEditarInteresse() {

    document
        .getElementById("modalEditarInteresse")
        .classList.add("hidden");
}

async function salvarEdicaoInteresse() {

    const valor =
        document.getElementById("novoValorInteresse").value;

    const interesse = interessesBanco[interesseEditando];

    const resposta = await fetch(`/api/admin/interesses/${interesse.id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            campo: campoInteresse,
            valor
        })
    });

    if (!resposta.ok) {
        const erro = await resposta.json().catch(() => null);
        alert(erro?.mensagem || "Erro ao editar interesse.");
        return;
    }

    fecharModalEditarInteresse();

    await carregarInteresses();

    abrirSucesso();
}

async function removerInteresse(id) {

    if (!confirm("Deseja remover este interesse?")) return;

    const resposta = await fetch(`/api/admin/interesses/${id}`, {
        method: "DELETE"
    });

    if (!resposta.ok) {
        const erro = await resposta.json().catch(() => null);
        alert(erro?.mensagem || "Erro ao remover interesse.");
        return;
    }

    await carregarInteresses();
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
        tipo === "admin"
            ? administradoresBanco
            : atendentesBanco;

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

async function salvarEdicaoCadastro() {

    const lista =
        tipoCadastro === "admin"
            ? administradoresBanco
            : atendentesBanco;

    const item = lista[cadastroIndex];

    if (!item) return;

    const rota =
        tipoCadastro === "admin"
            ? "administradores"
            : "atendentes";

    const resposta = await fetch(`/api/admin/${rota}/${item.id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            campo: campoCadastro,
            valor: document.getElementById("valorEditarCadastro").value
        })
    });

    if (!resposta.ok) {
        const erro = await resposta.json().catch(() => null);
        alert(erro?.mensagem || "Erro ao salvar cadastro.");
        return;
    }

    fecharModalEditarCadastro();

    await carregarAdministradores();

    await carregarAtendentes();

    abrirSucesso();
}

async function removerCadastro(tipo, id) {

    if (!confirm("Deseja remover este cadastro?")) return;

    const rota =
        tipo === "admin"
            ? "administradores"
            : "atendentes";

    const resposta = await fetch(`/api/admin/${rota}/${id}`, {
        method: "DELETE"
    });

    if (!resposta.ok) {
        const erro = await resposta.json().catch(() => null);
        alert(erro?.mensagem || "Erro ao remover cadastro.");
        return;
    }

    await carregarAdministradores();

    await carregarAtendentes();
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

async function salvarNovoUsuario() {

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

    if (!nome || !email || !senha) {
        alert("Preencha todos os campos.");
        return;
    }

    const rota =
        tipo === "admin"
            ? "administradores"
            : "atendentes";

    const resposta = await fetch(`/api/admin/${rota}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            nome,
            email,
            senha
        })
    });

    if (!resposta.ok) {
        const erro = await resposta.json().catch(() => null);
        alert(erro?.mensagem || "Erro ao criar usuário.");
        return;
    }

    fecharModalCriarUsuario();

    await carregarAdministradores();

    await carregarAtendentes();

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
async function sairPerfil() {
    await fetch("/api/auth/sair", { method: "POST" }).catch(() => null);
    window.location.href = "/Home/Index";
}
