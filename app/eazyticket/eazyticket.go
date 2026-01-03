package eazyticket

import (
	"os"

	"github.com/mwprogrammer/flow"
)

var settings flow.FlowSettings
var app *flow.Flow

func Setup() {

	settings = flow.FlowSettings{

		Id:      os.Getenv("WHATSAPP_BUSINESS_ID"),
		Version: os.Getenv("WHATSAPP_API_VERSION"),
		Token:   os.Getenv("WHATSAPP_TOKEN"),
		Sender:  os.Getenv("WHATSAPP_SENDER"),
	}

	app = flow.New(settings)

}

func ParseEvent(event string) (*flow.Message, error) {

	message, err := app.ParseMessage(event)

	if err != nil {
		return nil, err
	}

	return message, nil

}

func WelcomeUser(message flow.Message) error {

	err := app.DisplayTypingIndicator(message.Id, message.PhoneNumber)

	if err != nil {
		return err
	}

	err = app.ReplyWithText(message.PhoneNumber, "Welcome to EazyTicket!", false)

	if err != nil {
		return err
	}

	return nil

}
