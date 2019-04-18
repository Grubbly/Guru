<template>
    <div class="currentgen">
        yo
        <line-chart :chart-data="data" :options="options"></line-chart>
    </div>
</template>

<script>
import Firebase from 'firebase'
import LineChart from './LineChart.js'

let config = {
    apiKey: "AIzaSyAozuUHLPuRtrwI196F0aI6jIHZoDoKhRs",
    authDomain: "guru-base.firebaseapp.com",
    databaseURL: "https://guru-base.firebaseio.com",
    projectId: "guru-base",
    storageBucket: "guru-base.appspot.com",
    messagingSenderId: "359051215949"
};

let app = Firebase.initializeApp(config)
let db = app.database()
let currentAgentsRef = db.ref("currentSession/0")

export default {
    name: 'CurrentGenChart',
    data() {
        return {
            data: null,
            bestScores: [],
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales:{
                yAxes:[{
                    ticks: {
                    suggestedMax: 100,
                    beginAtZero: true
                    }
                }]
                }
            },
        }
    },
    components: {
      LineChart
    },
    firebase: {
        currentAgents: currentAgentsRef,
    },
    created() {
        db.ref("currentSession");
        db.ref().on('value',(snapshot) => {
            let currentGenerations = snapshot.val().currentSession
            let genLabels = []
            currentGenerations.forEach((gen,index) => {
                let min = Infinity
                genLabels.push("Gen " + index)
                gen.forEach(bot => {
                    min = Math.min(min, bot.score)
                })
                this.bestScores.push(100/min)
            });
            this.bestScores
            console.log("GenLabels: ", genLabels)

            this.data = {
                labels: genLabels,
                datasets: [{
                    label: "Best Score",
                    backgroundColor: "red",
                    data: this.bestScores
                }]
            }
        })
    }
}
</script>

<style>

</style>
