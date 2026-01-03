package main

import (
	"eazyticket/eazyticket"
	"encoding/json"
	"io"
	"log"
	"net/http"
	"os"

	"github.com/gin-gonic/gin"
	"github.com/joho/godotenv"
)

func main() {

	err := godotenv.Load()
	if err != nil {
		panic("Error loading .env file")
	}

	eazyticket.Setup()

	router := gin.Default()

	router.GET("/", func(context *gin.Context) {
		context.JSON(http.StatusOK, gin.H{"message": "Welcome to eazy ticket!"})
	})

	router.GET("/message", func(context *gin.Context) {

		mode := context.Query("hub.mode")
		token := context.Query("hub.verify_token")
		challenge := context.Query("hub.challenge")

		if mode == "subscribe" && token == os.Getenv("WHATSAPP_VERIFICATION_TOKEN") {
			context.String(http.StatusOK, challenge)
		} else {
			context.JSON(http.StatusForbidden, "VERIFICATION_FAILED")
		}

	})

	router.POST("/message", func(context *gin.Context) {

		defer context.Request.Body.Close()

		event, err := io.ReadAll(context.Request.Body)

		if err != nil {

			log.Println("Error reading request body:", err)
			context.JSON(http.StatusInternalServerError, "INTERNAL_SERVER_ERROR")

			return
		}

		message, err := eazyticket.ParseEvent(string(event))

		if err != nil {

			log.Println("Error parsing event as message:", err)
			context.JSON(http.StatusInternalServerError, "INTERNAL_SERVER_ERROR")

			return
		}

		debug, _ := json.Marshal(message)

		log.Println("Received message:", string(debug))

		err = eazyticket.WelcomeUser(*message)

		if err != nil {

			log.Println("Error sending welcome message:", err)
			context.JSON(http.StatusInternalServerError, "INTERNAL_SERVER_ERROR")

		}

		context.JSON(http.StatusOK, "EVENT_PROCESSED")

	})

	router.Run(":9090")

}
