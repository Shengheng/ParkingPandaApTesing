// JavaScript source code
$(function () {
    $(document).ready(function () {
        //disable past day for start date
        $("#startDate").attr('min', inputDateFormat(new Date()));

        //get week day pattern, stored in array
        var weekday = new Array(7);
        weekday[0] = "Sunday";
        weekday[1] = "Monday";
        weekday[2] = "Tuesday";
        weekday[3] = "Wednesday";
        weekday[4] = "Thursday";
        weekday[5] = "Friday";
        weekday[6] = "Saturday";

        var pattern = new Array();
        $("#weekday_selector table tr td").on("click", function () {
            var index = pattern.indexOf($(this).html());
            if (index != -1) {
                pattern.splice(index, 1);
                $(this).css("color", "black");
            }
            else {
                pattern.push($(this).html());
                $(this).css("color", "blue");
            };
        });

        //get start date         
        var startDate;
        $("#startDate").on("change", function () {

            startDate = JSDateFormat($(this).val());
            //enable end date input
            $("#endDate").attr('disabled', false);
            //disable pass date of start date for end date input
            $("#endDate").attr('min', new String($(this).val()));

            //change endDate if checkbox is checked
            if ($("#endDateCheck").is(':checked')) {
                var yr = startDate.getFullYear();
                var mon = startDate.getMonth();
                endDate = new Date(yr, mon + 1, 0);

                $("#endDate").val(inputDateFormat(endDate));
            }

        });

        //get end date
        var endDate;
        $("#endDate").on("change", function () {
            endDate = JSDateFormat($(this).val());
            //set checkbox to unchecked
            $("#endDateCheck").prop("checked", false);
        });

        //check box to select enddate
        $("#endDateCheck").on("change", function () {
            if (this.checked && startDate != null) {
                var yr = startDate.getFullYear();
                var mon = startDate.getMonth();
                endDate = new Date(yr, mon + 1, 0);

                $("#endDate").val(inputDateFormat(endDate));
            }
        });

        //date formater - formate -> "YYYY-MM-DD" ==> (int) year, month , day
        function JSDateFormat(input) {
            var dashOne = input.indexOf('-');
            var dashTwo = input.lastIndexOf('-');
            var year = parseInt(input.substring(0, dashOne), 10);
            var mon = parseInt(input.substring(dashOne + 1, dashTwo), 10);
            var day = parseInt(input.substring(dashTwo + 1), 10);

            return new Date(year, mon - 1, day);
        };

        //get current date
        function inputDateFormat(input) {
            var day = input.getDate();
            var mon = new String(input.getMonth() + 1);
            var yr = input.getFullYear();

            if (day.toString().length < 2) { day = "0" + day }
            if (mon.toString().length < 2) { mon = "0" + mon }
            return new String(yr + '-' + mon + '-' + day)
        };

        $("#commit").on("click", function () {
            if (pattern.length > 0) {
                for (var i = startDate; i <= endDate; i.setDate(i.getDate() + 1)) {
                    var len = pattern.length;
                    for (var j = 0; len > 0; j++, len--) {
                        if (weekday[i.getDay()] === pattern[j]) {
                            $("#result_tb").append("<tr><td>" + pattern[j] + " " + inputDateFormat(i) + "</td></tr>");
                        }
                    }
                }
            }
        });

        $("#reset").on("click", function () {
            pattern = new Array();
            $("#weekday_selector table tr td").css("color", "black");
            $("#endDate").val("mm/dd/yyyy");
            $("#endDate").attr('disabled', true);
            $("#endDateCheck").prop("checked", false);
            $("#startDate").val("mm/dd/yyyy");
            $("#result_tb tr").remove();
        });
    });
});
