<template>

  
</template>

<script>
import Axios from 'axios'
export default {
  name: 'HelloWorld',
  data(){
    return {
      isMorning: null,
      timeToNextMorningText: null,
      created: null,
      responseText: null
    }
  },
  created(){
    Axios.get("https://istitmorninginvietnamapi.azurewebsites.net/getifitismorninginvietnam")
    .then((response) => {
      this.created=true
      this.responseText = response
      this.isMorning = response.data.isMorning
      this.timeToNextMorningText = response.data.timeToNextSunriseText
    })
    
    
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
#not-morning{
  background-image:linear-gradient(#c9c9c9,#c9c9c9, #ffffff);
}
#not-morning h1{
  font-size: 72pt;
}
.centerwrapper{
  width:100%;
  height:100%;
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
