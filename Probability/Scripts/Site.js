import axios from 'axios';
import qs from 'qs';

//export default {
//    layout: 'blank-layout',
//    data: () => ({
//        calculatorOptions: [
//            { text: "Select your calculator", value: "" },
//            { text: "Combine", value: "Combine" },
//            { text: "Either", value: "Either" }
//        ],
//        modelState: {},
//        form: {
//            Calculator: '',
//            Left: '',
//            Right: ''
//        }
//    }),
//    computed: {
//        isSubmitDisabled() {
//            let isDisabled = true;

//            if (this.Calculator !== "" && this.Left !== "" && this.Right !== "") {
//                isDisabled = false;
//            }

//            return isDisabled;
//        }
//    },
//    methods: {
//        resetForm() {
//            this.data.form.Calculator = "";
//            this.data.form.Left = "";
//            this.data.form.Right = "";
//        },
//        async runCalculation() {
//            this.modelState = {},
//                this.data = await axios
//                    .post('/calculate',
//                        qs.stringify(this.form),
//                        {
//                            headers: {
//                                'Content-Type': 'application/x-www-form-urlencoded'
//                            }
//                        })
//                    .then(response => {
//                        alert('you logged in!');
//                    })
//                    .catch(error => {
//                        if (error.response.status == 400) {
//                            this.modelState = JSON.parse(error.response.data);
//                        }
//                    });
//        }
//    }
//};

//new Vue({
//        el:"#calculator-form",
//        data: {
//        CalculatorOptions: [
//            { text:"Select your calculator", value: "" },
//            { text:"Combine", value: "Combine" },
//            { text:"Either", value: "Either" },
//        ],
//        form: new 
//        Calculator: "",
//        Left: "",
//        Right: ""
//    },

new Vue({
    el: "#calculator-form",
    data: {
        modelState: {},
        CalculatorOptions: [
            { text:"Select your calculator", value: "" },
            { text:"Combine", value: "Combine" },
            { text:"Either", value: "Either" }
        ],
        Calculator: "",
        Left: "",
        Right: ""
    },
    computed: {
        isSubmitDisabled() {
            let isDisabled = true;

            if (this.Calculator !== "" && this.Left !== "" && this.Right !== "") {
                isDisabled = false;
            }

            return isDisabled;
        }
    },
    methods: {
        ResetForm() {
            this.Calculator = "";
            this.Left = "";
            this.Right = "";
        },
        RunCalculation() {
            axios.post({
                method: "post",
                url: "calculate",
                data: { "Fields": this.$data }
            }).then(res => {
                alert("Successfully submitted feedback form ");
                this.$refs.SubmitButton.setAttribute("disabled", "disabled");
            }).catch(error => {
                if (error.response.status === 400) {
                    this.modelState = JSON.parse(error.response.data);
                } else {
                    alert(`There was an error submitting your form. See details: ${err}`);
                }
            });
        }
    }
});