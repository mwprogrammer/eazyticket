package main

import (
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

	verificationToken := os.Getenv("META_VERIFY_TOKEN")
	if verificationToken == "" {
		panic("META_VERIFY_TOKEN not set in .env file")
	}

	router := gin.Default()

	router.GET("/", func(context *gin.Context) {
		context.JSON(http.StatusOK, gin.H{"message": "Welcome to eazy ticket!"})
	})

	router.GET("/eazyticket", func(context *gin.Context) {

		mode := context.Query("hub.mode")
		token := context.Query("hub.verify_token")
		challenge := context.Query("hub.challenge")

		if mode == "subscribe" && token == "my_verify_token" {
			context.String(http.StatusOK, challenge)
		} else {
			context.JSON(http.StatusForbidden, "Verification failed")
		}
	})

	router.POST("/eazyticket", func(context *gin.Context) {

		// TODO: Read incoming webhook data and process it using flow library

		context.JSON(http.StatusOK, "EVENT_RECEIVED")

	})

	router.Run(":9000")

}
