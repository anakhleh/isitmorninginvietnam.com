<template>
  <div id="app">
    <HelloWorld id="center-content-wrapper"/>
  </div>
</template>

<script>
import HelloWorld from './components/HelloWorld.vue'

export default {
  name: 'app',
  components: {
    HelloWorld
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

#center-content-wrapper{

  /* background-color: red; */
  position: absolute;
  top:0;
  bottom: 0;
  left: 0;
  right: 0;	
  margin: auto;
}
</style>
