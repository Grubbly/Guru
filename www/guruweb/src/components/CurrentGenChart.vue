<template>
    <div class="currentgen">
        <line-chart class="orange lighten-3 botChart" :chart-data="data" :options="options"></line-chart>
        
        <scatter-chart class="orange lighten-3 botChart" :chart-data="slashData" :options="options3D"></scatter-chart>
    </div>
</template>

<script>
import Firebase from 'firebase'
import LineChart from './LineChart.js'
import ScatterChart from './ScatterChart.js'

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
let currentAgentsRef = db.ref("currentSession")

export default {
    name: 'CurrentGenChart',
    data() {
        return {
            data: {},
            slashData: {},
            bestScores: [],
            options: {
                title: {
                    display: true,
                    text: 'Evolution History'
                },
                responsive: true,
                maintainAspectRatio: false,
                scales:{
                yAxes:[{
                    ticks: {
                        suggestedMax: 100,
                        beginAtZero: true,
                        min: 0
                    }
                }]
                }
            },
            options3D: {
                title: {
                    display: true,
                    text: 'Current Sword Slash Pattern'
                },
                responsive: true,
                maintainAspectRatio: false,
                scales:{
                yAxes:[{
                    ticks: {
                        beginAtZero: true,
                        min: 0
                    }
                }]
                }
            },
        }
    },
    components: {
      LineChart,
      ScatterChart,
    },
    firebase: {
        currentAgents: currentAgentsRef,
    },
    created() {

        let slashesRef = app.database().ref("slashVectors");

        slashesRef.on('value', (snapshot) => {
            let xyVec = []
            let slashLabels = []
            snapshot.val().slashVectors.forEach((vector,index) => {
                xyVec.push({
                    x: vector.x,
                    y: vector.y,
                })
                slashLabels.push(index)
            })
            this.slashData = {
                label: "Sword Slash",
                datasets: [{
                    label: "Current Sword Slash Pattern",
                    backgroundColor: "green",
                    data: xyVec
                }]
            }
        })

        db.ref("currentSession");
        db.ref().on('value',(snapshot) => {
            this.bestScores = []
            let currentGenerations = snapshot.val().currentSession
            if(currentGenerations !== undefined) {
                let genLabels = []
                currentGenerations.forEach((gen,index) => {
                    let min = Infinity
                    genLabels.push("Gen " + index)
                    Object.values(gen).forEach(bot => {
                        min = Math.min(min, bot.score)
                    })
                    this.bestScores.push(100/min)
                });
                this.data = {
                    labels: genLabels,
                    datasets: [{
                        label: "Best Score",
                        backgroundColor: "red",
                        data: this.bestScores
                    }]
                }
            }
        })
    }
}
</script>

<style>
    .botChart {
        margin-bottom: 15px;
    }
</style>
