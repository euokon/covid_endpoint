<template>
  <h1 id="tableLabel">COVID Results</h1>

  <p v-if="!covidResults"><em>Loading...</em></p>

  <!-- covidResults:{{ covidResults }} -->
  <table
    class="table table-striped"
    aria-labelledby="tableLabel"
    v-if="covidResults"
  >
    <thead>
      <tr>
        <th>State</th>
        <th>Confirmed Cases</th>
        <th>Cases on Admission</th>
        <th>Discharged</th>
        <th>Death</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="covidData of covidResults" v-bind:key="covidData">
        <!-- <td align="left">{{ covidData.state }}</td> -->
        <td>{{ covidData.state }}</td>
        <td>{{ covidData.confirmedCases }}</td>
        <td>{{ covidData.casesOnAdmission }}</td>
        <td>{{ covidData.discharged }}</td>
        <td>{{ covidData.death }}</td>
      </tr>
    </tbody>
  </table>
</template>


<script>
import axios from "axios";
export default {
  name: "CovidData",
  data() {
    return {
      covidResults: [],
    };
  },
  methods: {
    getCovidResults() {
      axios
        .get("/api/covid/fbn-covid-data")
        .then((response) => {
          this.covidResults = response.data;
        })
        .catch(function (error) {
          alert(error);
        });
    },
  },
  mounted() {
    this.getCovidResults();
  },
};
</script>