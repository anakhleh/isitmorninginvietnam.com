<template>
  <div id="#app" class="center-content-wrapper">
    <div v-if="isMorning==null">
    </div>
    <div v-else-if="isMorning" id="is-morning">
      <video autoplay class="video">
        <source src="./assets/videoplayback.mp4" type="video/mp4">
      </video>
      <div class="content">
        It's 8:30 AM{{currentTimeText}} in Vietnam.
      </div>
    </div>
    <div v-else id="not-morning">
      <div class="NO">NO</div>
      <div class="But-if-you-come-back">
        But if you come back in about {{this.timeToNextMorningText}}, it just might be.
      </div>
    </div>
  </div>
</template>

<script>
import Axios from 'axios'

export default {
  name: 'app',
  components: {
  },
    data(){
    return {
      isMorning: null,
      timeToNextMorningText: null,
      currentTimeText: null,
    }
  },
  created(){
    Axios.get("https://istitmorninginvietnamapi.azurewebsites.net/getifitismorninginvietnam")
    .then((response) => {
      this.isMorning = response.data.isMorning
      this.timeToNextMorningText = response.data.timeToNextSunriseText
      this.currentTimeText = response.data.currentTimeText
    })
    
    
  },
  computed(){
    return {

    }
  }
}
</script>

<style>
#app {  
}
.center-content-wrapper{
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  font-family: acumin-pro-wide, sans-serif;
  font-weight: 100;
  font-style: normal;
  text-align: center;
  height: 100%;
  display: flex;
  /* justify-content: center; */
  /* align-items: center; */
  height: 100vh;
}
#is-morning{
  
}
.video{
  position: fixed;
  right: 0;
  bottom: 0;
  min-width: 100%;
  min-height: 100%;
}
.content {
  position: fixed;
  bottom: 0;
  left:0;
  background-image:linear-gradient(rgba(0,0,0,0), rgba(0,0,0,1));
  color: #f1f1f1;
  min-width: 100%;
  padding: 20px;
  opacity: 1;
	animation-name: fadeInOpacity;
	animation-iteration-count: 1;
	animation-timing-function: ease-in;
	animation-duration: 0.5s;
}

#not-morning{
  opacity: 1;
	animation-name: fadeInOpacity;
	animation-iteration-count: 1;
	animation-timing-function: ease-in;
	animation-duration: 0.5s;
}
.NO{
  font-size: 144pt;
}
.But-if-you-come-back{
  font-weight:300;
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


@keyframes fadeInOpacity {
	0% {
		opacity: 0;
	}
	100% {
		opacity: 1;
	}
}
</style>
