<template>
  <div class="chart">
    <h1>{{ msg }}</h1>
      <div class="frappe card grey lighten-3 botChart" v-for="(botData,index) in this.data" :key="index">
          <h1> {{chartTimestamps[index]}} </h1>
          <line-chart :chart-data="botData" :options="options"></line-chart>
      </div>
  </div>
</template>

<script>
import LineChart from './LineChart.js'
import axios from 'axios'

export default {
  name: 'Chart',
  props: {msg: String},
  components: {
      LineChart
  },
  data () {
      return {
          data: [],
          labels: [],
          bot: null,
          DAYFILTER: 20,
          chartTimestamps: [],
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
          }
      }
  },
  created() {
    for (let index = 0; index < 16; index++) {
      this.labels.push("Bot " + index)
    }

    axios.get('https://guru-base.firebaseio.com/archivedSessions.json').then((month) => {
      if(month !== undefined) 
        Object.values(month).forEach(day => {
          if(day !== undefined)
          Object.values(day).forEach(date => {
            if(date !== undefined)
            Object.values(date).forEach(gen => {
              Object.values(gen).forEach(bot => {
                  Object.values(bot).forEach(botGen => {
                      if(botGen[0].dnaLength) {
                        this.data.push({
                          labels: this.labels,
                          datasets: [{
                            label: "Score",
                            backgroundColor: "orange",
                            data: []
                          }]
                        })
                        Object.values(botGen).forEach(botData => {
                          this.data[this.data.length-1].datasets[0].data.push(100/botData.score)
                        })
                        this.chartTimestamps.push(Object.keys(gen)[this.data.length-1])
                      }
                  })
              })
            })
          })
        });
    }).then(() => {
      this.data.reverse()
      this.chartTimestamps.reverse()
    })
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

.chart {
  text-align: center;
}

.chart .botChart {
  width: 60%;
  margin: 0 auto;
}

h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
