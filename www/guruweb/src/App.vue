<template>
  <div id="app">
    <img alt="Guru logo" src="./assets/logo.png">
    <h1>Guru</h1>
    <p class="grey lighten-3 card author">Tristan Van Cise</p>

    <div class="container">
      <h1>Current Generation</h1>
      <div class="orange lighten-3">
        yo
        <CurrentGenChart />
      </div>
      <Chart msg="Archived Generations"/>
    </div>
  </div>
</template>

<script>
import Chart from './components/Chart.vue'
import CurrentGenChart from './components/CurrentGenChart.vue'

export default {
  name: 'app',
  components: {
    Chart,
    CurrentGenChart,
  },
  data() {
    return {

    }
  }
}
</script>

<style>

.content{
  max-width: 90%;
  width: 50%;
  margin: 0 auto;
}

.author{
  max-width: 10%;
  width: 50%;
  margin: 0 auto;
}

#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
