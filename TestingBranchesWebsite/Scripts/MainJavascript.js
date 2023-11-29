function login() {
    let userInput = document.getElementById("username").value;
    let passInput = document.getElementById("password").value;

    $.ajax({
        type: "POST",
        url: "/Data/CheckLogin",
        data: { username: userInput, password: passInput },
        success: function (response) {
            if (response.success) {
                //go to home page
                //isLogged = true;
                sessionStorage.setItem("isLogged", "true");
                window.location.href = "/Home/Branches";
            } else {
                document.getElementById("login-outcome").innerHTML = "Incorrect username or password";
            }
        },
        error: function () {
            document.getElementById("login-outcome").innerHTML = "An error occurred";
        }
    });
}


//if user is in branches (home page)
if (document.URL.toLowerCase().includes("home/branches")) {
    loadBranches();
}

function loadBranches() {
    var isLogged = sessionStorage.getItem("isLogged");
    //check if user bypassed login page. if not logged in, take user to login page
    if (isLogged == "true") {
        //load branches
        const branches = ["Test_Example", "Branch789", "Master"]; //TODO add new function to get branches and store them here
        branches.forEach(createBranch);
    } else {
        window.location.href = "/";
    }
}

function createBranch(item) {
    var branchesContainer = document.getElementById("branches");

    var containerDiv = document.createElement("div");
    containerDiv.classList.add("container", "branch");

    var title = document.createElement("p");
    title.textContent = item;

    var hr = document.createElement("hr");
    hr.classList.add("container", "branch-divider");

    var branchStatusDiv = document.createElement("div");
    branchStatusDiv.id = "branch-status";

    var detailsButton = document.createElement("button");
    detailsButton.classList.add("container", "branch-details");
    detailsButton.textContent = "Details";

    var runButton = document.createElement("button");
    runButton.classList.add("container", "branch-run");
    runButton.textContent = "Run";

    containerDiv.appendChild(title);
    containerDiv.appendChild(hr);
    containerDiv.appendChild(branchStatusDiv);
    containerDiv.appendChild(detailsButton);
    containerDiv.appendChild(runButton);

    document.body.appendChild(containerDiv);
    branchesContainer.appendChild(containerDiv);

    var status = document.getElementById("branch-status").style;

    //if branch passed
    if (true) {
        //set status led to green
        status.cssText = "background-color: #00ff82; box-shadow: rgba(0, 0, 0, 0.2) 0 -1px 7px 1px, inset #3bc868 0 -1px 9px, #89FF00 0 2px 12px;";
    }
}