const ModeloBase = {
    idUsuario: 0,
    nombre: "",
    correo: "",
    telefono: "",
    idRol: 0,
    esActivo: 1,
    urlFoto: ""
}

let TablaData;
$(document).ready(function () {

    fetch("/Usuario/ListaRoles").then(response => {
        return response.ok ? response.json() : Promise.reject(response)
    }).then(responseJson => {
        if (responseJson.length > 0) {
            responseJson.forEach((item) => {
                $("#cboRol").append($("<option>").val(item.idRol).text(item.descripcion))
            })
        }
    })

    TablaData= $('#tbdata').DataTable({
        responsive: true,
         "ajax": {
             "url": '/Usuario/ListaUsuarios',
             "type": "GET",
             "datatype": "json"
         },
        "columns": [
            { "data": "idUsuario", "visible": false, "searchable": false },
            {
                "data": "urlFoto", render: function (data) {//aquí se usa tilde invertida
                    return `<img style="height:60px" src=${data} class="rounded mx-auto d-block"/>`
                }
            },
             { "data": "nombre" },
             { "data": "correo" },
             { "data": "telefono" },
            { "data": "nombreRol" },
            {
                "data": "esActivo", render: function (data)
                {
                    if (data == 1) {
                        return '<span class="badge badge-info">Activo</span>';
                    } else {
                        '<span class="badge badge-danger">No Activo</span>';
                    }
                }
            },
             {
                 "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                     '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                 "orderable": false,
                 "searchable": false,
                 "width": "80px"
             }
         ],
         order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Lista Usuarios',
                exportOptions: {
                    columns: [2,3,4,5,6]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})

function MostrarUserNew(modelo = ModeloBase) {
    $("#txtId").val(modelo.idUsuario)
    $("#txtNombre").val(modelo.nombre)
    $("#txtCorreo").val(modelo.correo)
    $("#txtTelefono").val(modelo.telefono)
    $("#cboRol").val(modelo.idRol == 0 ? $("#cboRol option:first").val() : modelo.idRol)
    $("#cboEstado").val(modelo.esActivo)
    $("#txtFoto").val("")
    $("#imgUsuario").attr("src", modelo.urlFoto)
    $("#modalData").modal("show")
}



$("#btnGuardar").click(function () {
    
    const inputs = $("input.input-validar").serializeArray();
    const inputs_vacios = inputs.filter((item) => item.value.trim() == "")

    if (inputs_vacios.length > 0) {
        const mensaje = `Debes llenar todos los campos: "${inputs_vacios[0].name}"`;
        toastr.warning("", mensaje)
        $(`input[name="${inputs_vacios[0].name}"]`).focus()
        return;
    }
    const modelo = structuredClone(ModeloBase);
    modelo["idUsuario"] = parseInt($("txtId").val())
    modelo["nombre"] = $("txtNombre").val()
    modelo["correo"] = $("txtCorreo").val()
    modelo["telefono"] = $("txtTelefono").val()
    modelo["idRol"] = $("cboRol").val()
    modelo["esActivo"] = $("cboEstado").val()

    const inputFoto = document.getElementById("txtFoto")

    const formData = new FormData();

    formData.append("foto", inputFoto.files[0])
    formData.append("modelo", JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");//Mostrar Vista de cargar

    if (modelo.idUsuario==0) {
        fetch("/Usuario/CrearUsuarios", {
            method: "POST",
            body: formData
        }).then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");//Ocultar Vista de cargar
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.estado==true) {
                TablaData.row.add(responseJson.objeto).draw(false)
                $("#modalData").modal("hide")
                swal("¡Listo!", "El usuario ha sido creado", "success")
            } else {
                swal("¡Hubo un fallo!", responseJson.mensaje, "error")
            }
        })
    }
})
$("#btnNuevo").click(function () {
    MostrarUserNew()
})