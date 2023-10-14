$(document).ready(function () {
    const projectSelectElement = $("#projectsSelect");
    const errorBlock = $("#errorBlock");
    const errorText = $(".error-text");
    const projectRow = $("#employeeRow");
    const projectLoader = $("#employeeLoader");

    if (projectSelectElement.length !== 0) {
        projectLoader.show();
        projectRow.hide();

        $.ajax({
            type: "GET",
            url: "/project/getSampleProjects",
            dataType: "json",
            success: function (response) {
                $.each(response, function (index, item) {
                    projectSelectElement.append(new Option(item.label, item.value));
                    console.log(item);
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

    const formAdd = $(".employee-action");

    formAdd.submit(function (e) {
        e.preventDefault();

        if (formAdd.valid()) {
            $.ajax({
                type: "POST",
                url: "/employee/create",
                data: formAdd.serialize(),
                success: function (e) {
                    window.location.replace("/Employee");
                },
                error: function (xhr, status, error) {
                    errorBlock.show();
                    errorText.text(error);
                }
            });
        }
    });

    const formEdit = $(".employee-edit");

    formEdit.submit(function (e) {
        e.preventDefault();

        const url = window.location.pathname;
        const id = url.substring(url.lastIndexOf("/") + 1);

        if (formEdit.valid()) {
            $.ajax({
                type: "PUT",
                url: "/employee/edit/" + id,
                data: formEdit.serialize(),
                success: function (e) {
                    window.location.replace("/Employee");
                },
                error: function (xhr, status, error) {
                    errorBlock.show();
                    errorText.text(error);
                }
            });
        }
    });

    const deleteEmployeeButton = $("#deleteEmployee");

    deleteEmployeeButton.click(function (e) {
        const url = window.location.pathname;
        const id = url.substring(url.lastIndexOf("/") + 1);

        $.ajax({
            type: "DELETE",
            url: "/employee/delete/" + id,
            success: function (response) {
                window.location.replace("/Employee");
            },
            error: function (xhr, status, error) {
                errorBlock.show();
                errorText.text(error);
            }
        });
    });
});