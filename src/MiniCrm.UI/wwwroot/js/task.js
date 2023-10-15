$(document).ready(function () {
    const projectTasksSelect = $("#projectTasks");
    const errorBlock = $("#errorBlock");
    const errorText = $(".error-text");
    const taskRow = $("#taskRow");
    const taskLoader = $("#taskLoader");
    const taskExecutorsSelect = $("#taskExecutors");
    const taskAuthorsSelect = $("#taskAuthors");

    if (projectTasksSelect.length !== 0 && taskExecutorsSelect.length !== 0) {
        taskLoader.show();
        taskRow.hide();

        const projects = $.ajax({
            type: "GET",
            url: "/project/getSampleProjects",
            dataType: "json",
        });

        const employees = $.ajax({
            type: "GET",
            url: "/employee/getSampleEmployees",
            dataType: "json",
        });

        $.when(projects, employees).done(function (data1, data2) {
            $.each(data1[0], function (index, item) {
                projectTasksSelect.append(new Option(item.label, item.value));
            });

            $.each(data2[0], function (index, item) {
                taskExecutorsSelect.append(new Option(item.label, item.value));
            });

            $.each(data2[0], function (index, item) {
                taskAuthorsSelect.append(new Option(item.label, item.value));
            });

            taskLoader.hide();
            taskRow.show();
        }).fail(function (xhr, status, error) {
            errorBlock.show();
            errorText.text(error);

            taskLoader.hide();
            taskRow.show();
        });
    }

    const taskAdd = $(".task-action");

    taskAdd.submit(function (e) {
        if (taskAdd.valid()) {
            e.preventDefault();

            $.ajax({
                type: "POST",
                url: "/task/create",
                data: taskAdd.serialize(),
                success: function (response) {
                    window.location.replace("/Task");
                },
                error: function (xhr, status, error) {
                    errorBlock.show();
                    errorText.text(error);

                    console.log(`Error: ${error}, Status: ${status}`);
                }
            });
        }
    });

    const taskEdit = $(".task-edit");

    taskEdit.submit(function (e) {
        if (taskEdit.valid()) {

            e.preventDefault();

            const url = window.location.pathname;
            const id = url.substring(url.lastIndexOf("/") + 1);

            $.ajax({
                type: "PUT",
                url: "/task/edit/" + id,
                data: taskEdit.serialize(),
                success: function (response) {
                    window.location.replace("/Task");
                },
                error: function (xhr, status, error) {
                    errorBlock.show();
                    errorText.text(error);

                    console.log(`Error: ${error}, Status: ${status}`);
                }
            });
        }
    });

    const deleteProjectButton = $("#deleteTask");

    deleteProjectButton.click(function (e) {
        const url = window.location.pathname;
        const id = url.substring(url.lastIndexOf("/") + 1);

        $.ajax({
            type: "DELETE",
            url: "/task/delete/" + id,
            success: function (response) {
                window.location.replace("/Task");
            },
            error: function (xhr, status, error) {
                errorBlock.show();
                errorText.text(error);
            }
        });
    });

    const taskStatusChange = $("#taskStatus");

    taskStatusChange.on("change", function (e) {
        $.ajax({
            type: "POST",
            url: "/task/changeTaskStatus",
            data: { "status" : this.value },
        });
    });
});