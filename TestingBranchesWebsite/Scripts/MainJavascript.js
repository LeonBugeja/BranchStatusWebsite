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


function fetchBranches(callback) {
    $.ajax({
        type: "GET",
        url: "/Data/CheckBranchStatus",
        success: function (response) {
            // Check if the response is not empty
            if (response && response.length > 0) {
                callback(response);
            } else {
                callback("No branches found");
            }
        },
        error: function () {
            callback("Error retrieving branches");
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
        fetchBranches(function (branches) {
            console.log(branches);
            branches.forEach(createBranch);
        });
    } else {
        window.location.href = "/";
    }
}

function createBranch(branch) {
    var branchesContainer = document.getElementById("branches");

    var containerDiv = document.createElement("div");
    containerDiv.classList.add("container", "branch");

    //the name of the branch
    var title = document.createElement("p");
    title.textContent = branch.Name;

    var hr = document.createElement("hr");
    hr.classList.add("container", "branch-divider");

    //the status led of the branch
    var branchStatusDiv = document.createElement("div");
    branchStatusDiv.className = "branch-status";

    //the details button
    var detailsButton = document.createElement("button");
    detailsButton.classList.add("container", "branch-details");
    detailsButton.textContent = "Details";

    //the run tests button
    var runButton = document.createElement("button");
    runButton.classList.add("container", "branch-run");
    runButton.textContent = "Run";

    //if branch passed
    if (branch.Status == 0) {
        //set the status led to a loading circle
        branchStatusDiv.id = "status-processing";
        console.log(branch.Name + "'s Tests are still Processing");
    } else if (branch.Status == 1) {
        //set status led to green
        branchStatusDiv.id = "status-passed";
        console.log(branch.Name + "'s Tests Passed");
    } else if (branch.Status == 2) {
        console.log(branch.Name + "'s Tests Failed");
    }

    //display the branch
    containerDiv.appendChild(title);
    containerDiv.appendChild(hr);
    containerDiv.appendChild(branchStatusDiv);
    containerDiv.appendChild(detailsButton);
    containerDiv.appendChild(runButton);

    document.body.appendChild(containerDiv);
    branchesContainer.appendChild(containerDiv);
}