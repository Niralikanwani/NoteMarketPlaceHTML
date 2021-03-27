/*==========================================
              Mobile Menu
==========================================*/
$(function () {

	// Open Mobile nav
	$("#mobile-nav-open-btn").click(function () {
		$("#mobile-nav").css("height", "100%");
	});
	
	// Close Mobile nav
	$("#mobile-nav-close-btn").click(function () {
		$("#mobile-nav").css("height", "0%");
	});

});

/*============================================
           Login/signUp/Change password
============================================*/

// Change type text to obscure text by clicking eye-image

/* Login page */
$("#login div.eye .eye-img").click(function () {
	var input1 = $("#password").attr("type");

	if (input1 == "password") {
		$("#password").attr("type", "text");
	} else {
		$("#password").attr("type", "password");
	}
});

/* Sign Up page */
$("#signup form .form-group:nth-child(4) div.eye .eye-img").click(function () {
	var input1 = $("#password").attr("type");

	if (input1 == "password") {
		$("#password").attr("type", "text");
	} else {
		$("#password").attr("type", "password");
	}
});

$("#signup form .form-group:nth-child(5) div.eye .eye-img").click(function () {
	var input1 = $("#confirmpassword").attr("type");

	if (input1 == "password") {
		$("#confirmpassword").attr("type", "text");
	} else {
		$("#confirmpassword").attr("type", "password");
	}
});

/* change Password page */
$("#change form .form-group:nth-child(1) div.eye .eye-img").click(function () {
	var input4 = $("#oldpassword").attr("type");

	if (input4 == "password") {
		$("#oldpassword").attr("type", "text");
	} else {
		$("#oldpassword").attr("type", "password");
	}
});

$("#change form .form-group:nth-child(2) div.eye .eye-img").click(function () {
	var input5 = $("#newpassword").attr("type");

	if (input5 == "password") {
		$("#newpassword").attr("type", "text");
	} else {
		$("#newpassword").attr("type", "password");
	}
});

$("#change form .form-group:nth-child(3) div.eye .eye-img").click(function () {
	var input6 = $("#confirmpassword").attr("type");

	if (input6 == "password") {
		$("#confirmpassword").attr("type", "text");
	} else {
		$("#confirmpassword").attr("type", "password");
	}
});