$(document).ready(function () {
    const managersSelectElement = $("#projectManagers");
    const employeesSelectElement = $("#projectEmployees");
    const projectTasks = $("#projectTasks");
    const errorBlock = $("#errorBlock");
    const errorText = $(".error-text");
    const projectRow = $("#projectRow");
    const projectLoader = $("#projectLoader");

    if (employeesSelectElement.length !== 0) {
        projectLoader.show();
        projectRow.hide();

        const employees = $.ajax({
            type: "GET",
            url: "/employee/getSampleEmployees",
            dataType: "json",
        });

        const tasks = $.ajax({
            type: "GET",
            url: "/task/getSampleTasks",
            dataType: "json",
        });

        $.when(tasks, employees).done(function (data1, data2) {
            $.each(data2[0], function (index, item) {
                employeesSelectElement.append(new Option(item.label, item.value));
            });

            $.each(data2[0], function (index, item) {
                managersSelectElement.append(new Option(item.label, item.value));
            });

            $.each(data1[0], function (index, item) {
                projectTasks.append(new Option(item.label, item.value));
            });

            projectLoader.hide();
            projectRow.show();
        }).fail(function (xhr, status, error) {
            errorBlock.show();
            errorText.text(error);

            projectLoader.hide();
            projectRow.show();
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
                    window.location.replace("/Project");
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
                    window.location.replace("/Project");
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
                window.location.replace("/Project");
            },
            error: function (xhr, status, error) {
                errorBlock.show();
                errorText.text(error);
            }
        });
    });
});