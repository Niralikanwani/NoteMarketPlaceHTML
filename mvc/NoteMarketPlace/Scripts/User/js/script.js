/*==========================================
			Mobile Menu
==========================================*/
$(function () {

	// Open mobile nav
	$("#mobile-nav-open-btn").click(function () {
		$("#mobile-nav").css("height", "100%");
	});
	
	// Close mobile nav
	$("#mobile-nav-close-btn").click(function () {
		$("#mobile-nav").css("height", "0%");
	});

});


/*==========================================
			Sticky Header (show and hide)
==========================================*/

// change logo on scroll window 
function sticky_header() {
	var header_height = jQuery('.site-header').innerHeight() / 2;
	var scrollTop = jQuery(window).scrollTop();
	
	if (scrollTop > header_height) {
		jQuery('body').addClass('sticky-header');
		$("#home-page .logo-wrapper img").attr("src", "~/Content/User/images/logo/dark-logo.png");
	} else {
		jQuery('body').removeClass('sticky-header');
		$("#home-page .logo-wrapper img").attr("src", "~/Content/User/images/logo/white-logo.png");
	}
}

jQuery(document).ready(function () {
	sticky_header();
});

jQuery(window).scroll(function () {
	sticky_header();
});

jQuery(window).resize(function () {
	sticky_header();
});


/*============================================
		   Login/signUp/Change password
============================================*/

// Change type text to obscure text by clicking eye-image

/* Login page */
$("#login form div.eye .eye-img").click(function () {
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

/* Change Password page */
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


/*============================================
		   FAQ page(open/close faq questions)
============================================*/

// 1
$("#faq .faq1 .faq-plus")[0].addEventListener("click", function () {
	$("#faq .faq1 .faq-plus")[0].style.display = "none";
	$("#faq .faq1 .faq-minus")[0].style.display = "block";
});

$("#faq .faq1 .faq-minus")[0].addEventListener("click", function () {
	$("#faq .faq1 .faq-minus")[0].style.display = "none";
	$("#faq .faq1 .faq-plus")[0].style.display = "block";
});

// 2

$("#faq .faq2 .faq-plus")[0].addEventListener("click", function () {
	$("#faq .faq2 .faq-plus")[0].style.display = "none";
	$("#faq .faq2 .faq-minus")[0].style.display = "block";
});

$("#faq .faq2 .faq-minus")[0].addEventListener("click", function () {
	$("#faq .faq2 .faq-minus")[0].style.display = "none";
	$("#faq .faq2 .faq-plus")[0].style.display = "block";
});

// 3

$("#faq .faq3 .faq-plus")[0].addEventListener("click", function () {
	$("#faq .faq3 .faq-plus")[0].style.display = "none";
	$("#faq .faq3 .faq-minus")[0].style.display = "block";
});

$("#faq .faq3 .faq-minus")[0].addEventListener("click", function () {
	$("#faq .faq3 .faq-minus")[0].style.display = "none";
	$("#faq .faq3 .faq-plus")[0].style.display = "block";
});

// 4

$("#faq .faq4 .faq-plus")[0].addEventListener("click", function () {
	$("#faq .faq4 .faq-plus")[0].style.display = "none";
	$("#faq .faq4 .faq-minus")[0].style.display = "block";
});

$("#faq .faq4 .faq-minus")[0].addEventListener("click", function () {
	$("#faq .faq4 .faq-minus")[0].style.display = "none";
	$("#faq .faq4 .faq-plus")[0].style.display = "block";
});

// 5

$("#faq .faq5 .faq-plus")[0].addEventListener("click", function () {
	$("#faq .faq5 .faq-plus")[0].style.display = "none";
	$("#faq .faq5 .faq-minus")[0].style.display = "block";
});

$("#faq .faq5 .faq-minus")[0].addEventListener("click", function () {
	$("#faq .faq5 .faq-minus")[0].style.display = "none";
	$("#faq .faq5 .faq-plus")[0].style.display = "block";
});

// 6

$("#faq .faq6 .faq-plus")[0].addEventListener("click", function () {
	$("#faq .faq6 .faq-plus")[0].style.display = "none";
	$("#faq .faq6 .faq-minus")[0].style.display = "block";
});

$("#faq .faq6 .faq-minus")[0].addEventListener("click", function () {
	$("#faq .faq6 .faq-minus")[0].style.display = "none";
	$("#faq .faq6 .faq-plus")[0].style.display = "block";
});

// 7

$("#faq .faq7 .faq-plus")[0].addEventListener("click", function () {
	$("#faq .faq7 .faq-plus")[0].style.display = "none";
	$("#faq .faq7 .faq-minus")[0].style.display = "block";
});

$("#faq .faq7 .faq-minus")[0].addEventListener("click", function () {
	$("#faq .faq7 .faq-minus")[0].style.display = "none";
	$("#faq .faq7 .faq-plus")[0].style.display = "block";
});