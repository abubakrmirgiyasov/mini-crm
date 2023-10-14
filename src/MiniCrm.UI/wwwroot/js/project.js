$(document).ready(function () {
    const employeesSelectElement = $("#projectEmployees");
    const errorBlock = $("#errorBlock");
    const errorText = $(".error-text");

    if (employeesSelectElement.length !== 0) {
        const managersSelectElement = $("#projectManagers");
        const projectRow = $("#projectRow");
        const projectLoader = $("#projectLoader");

        projectLoader.show();
        projectRow.hide();

        $.ajax({
            type: "GET",
            url: "/project/getEmployees",
            dataType: "json",
            success: function (response) {
                $.each(response, function (index, item) {
                    employeesSelectElement.append(new Option(item.label, item.value));
                    console.log(item);
                });

                $.each(response, function (index, item) {
                    managersSelectElement.append(new Option(item.label, item.value));
                });

                projectLoader.hide();
                projectRow.show();
            },
            error: function (xhr, status, error) {
                projectLoader.hide();
                projectRow.show();

                errorBlock.show();
                errorText.text(error);

                console.log(`Error: ${error}, Status: ${status}`);
            }
        });
    }

    const formAdd = $(".project-action");

    formAdd.submit(function (e) {
        e.preventDefault();

        if (formAdd.valid()) {
            $.ajax({
                type: "POST",
                url: "/project/create",
                data: formAdd.serialize(),
                success: function (response) {
                    window.location.replace("/project");
                },
                error: function (xhr, status, error) {
                    errorBlock.show();
                    errorText.text(error);

                    console.log(`Error: ${error}, Status: ${status}`);
                }
            });
        }
    });

    const formEdit = $(".project-edit");

    formEdit.submit(function (e) {
        e.preventDefault();

        const url = window.location.pathname;
        const id = url.substring(url.lastIndexOf("/") + 1);

        if (formEdit.valid()) {
            $.ajax({
                type: "PUT",
                url: "/project/edit/" + id,
                data: formEdit.serialize(),
                success: function (e) {
                    window.location.replace("/project");
                },
                error: function (xhr, status, error) {
                    errorBlock.show();
                    errorText.text(error);
                }
            });
        }
    });

    const deleteProjectButton = $("#deleteProject");

    deleteProjectButton.click(function (e) {
        const url = window.location.pathname;
        const id = url.substring(url.lastIndexOf("/") + 1);

        $.ajax({
            type: "DELETE",
            url: "/project/delete/" + id,
            success: function (response) {
                window.location.replace("/");
            },
            error: function (xhr, status, error) {
                errorBlock.show();
                errorText.text(error);
            }
        });
    });
});