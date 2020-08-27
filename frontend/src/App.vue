<template>
  <div
    id="app"
    class="center-content-wrapper"
  >
    <div
      v-if="isFailure==true"
      class="failure-message"
    >
      It seems that isitmorninginvietnam.alexnakhleh.com is currently down.
      <br>
      <br>
      Please <a
        href="mailto:alex@alexnakhleh.com"
        style="mailto-alex"
      ><i><b><u>email Alex at alex@alexnakhleh.com</u></b></i></a> to let him know.
    </div>
    <div v-else-if="isMorning==null && isFailure==false">
      <div class="vietnam-loading-spinner">

      </div>
      <!-- Do nothing until response -->
    </div>
    <div
      v-else-if="isMorning == true"
      id="is-morning"
    >
      <iframe
        width="755"
        height="425"
        src="https://www.youtube.com/embed/BIikfdNIHQE?autoplay=1&controls=0&loop=1&modestbranding=1&disablekb=1%html5=True"
        frameborder="0"
        allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture"
        allowfullscreen
        id="video"
      ></iframe>
      <div class="content">
        It's {{this.currentTimeText}} in Vietnam.
      </div>
    </div>
    <div
      v-else
      id="not-morning"
    >
      <div class="NO">NO</div>
      <div class="But-if-you-come-back">
        But if you come back in about {{this.timeToNextMorningText}}, it just might be.
      </div>
    </div>
  </div>
</template>

<script>
import Axios from "axios";

export default {
  name: "app",
  components: {},
  data() {
    return {
      isMorning: null,
      timeToNextMorningText: null,
      currentTimeText: null,
      isFailure: false
    };
  },
  created() {
    //check whether it is morning, and the time to next morning, and current time text
    //but you only get the current time if it is morning, and time to next morning if it is not morning
    Axios.get(
      "https://istitmorninginvietnamapi.azurewebsites.net/getifitismorninginvietnam"
    )
      .then(response => {
        console.log(response);
        if (response.data.isMorning === null) {
          this.isFailure = true;
        } else {
          this.isMorning = response.data.isMorning;
          this.timeToNextMorningText = response.data.timeToNextSunriseText;
          this.currentTimeText = response.data.currentTimeText;
        }
      })
      .catch(e => {
        console.log(e);
        this.isFailure = true;
      });
  },
  ready() {
    //check the time every 5 minutes
    window.setInterval(() => {
      Axios.get(
        "https://istitmorninginvietnamapi.azurewebsites.net/getifitismorninginvietnam&showinfo=0"
      )
        .then(response => {
          this.isMorning = response.data.isMorning;
          this.timeToNextMorningText = response.data.timeToNextSunriseText;
          this.currentTimeText = response.data.currentTimeText;
        })
        .catch(() => {});
    }, 5000);
  },
  methods: {
    play: function() {
      document.getElementById("video").click();
      document.getElementById("video").play();
    }
  },
  computed: {
    morning() {
      return this.isMorning;
    }
  }
};
</script>

<style>
@import url("https://use.typekit.net/mai4xwg.css");
#app {
}
.vietnam-loading-spinner {
  background: red;
  width: 300px;
  height: 300px;
}
.failure-message {
  font-family: acumin-pro-wide, sans-serif;
  text-align: center;
  font-size: 15pt;
  font-weight: 300;
}
.mailto-alex {
}
.center-content-wrapper {
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  font-weight: 100;
  font-style: normal;
  text-align: center;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
}
#video {
  position: fixed;
  bottom: 0;
  left: 0;
  min-width: 100%;
  min-height: 100%;
}
.content {
  position: fixed;
  bottom: 0;
  left: 0;
  background-image: linear-gradient(rgba(0, 0, 0, 0), rgba(0, 0, 0, 1));
  color: #f1f1f1;
  min-width: 100%;
  padding: 20px;
  opacity: 1;
  animation-name: fadeInOpacity;
  animation-iteration-count: 1;
  animation-timing-function: ease-in;
  animation-duration: 0.5s;
}

#not-morning {
  font-family: acumin-pro-wide, sans-serif;
  font-weight: 300;
  opacity: 1;
  animation-name: fadeInOpacity;
  animation-iteration-count: 1;
  animation-timing-f3unction: ease-in;
  animation-duration: 0.5s;
}
.NO {
  font-size: 144pt;
}
.But-if-you-come-back {
  font-weight: 400;
  font-style: normal;
}
.centerwrapper {
  width: 100%;
  height: 100%;
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
@media (pointer: coarse) {
  #video {
    display: none;
  }
  #videomobile {
    position: fixed;
    bottom: 0;
    left: 0;
    min-width: 100%;
    min-height: 100%;
  }
}

@media (hover: none) {
  #video {
    display: none;
  }
  #videomobile {
    /* position: fixed; */
    /* bottom: 0; */
    /* left:0; */
    /* min-width: 100%; */
    /* min-height: 100%; */
  }
}
</style>
