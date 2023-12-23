package main

import (
	"bytes"
	"fmt"
	"os"
	"slices"
	"strconv"
	"time"
)

func main() {
	file, err := os.ReadFile("day7/input7.txt")
	if err != nil {
		fmt.Println(err.Error())
	}

	startTime := time.Now()
	answer := part1(file)
	endTime := time.Now()
	elapsedTime := endTime.Sub(startTime)
	fmt.Println("Part 1 Time:", elapsedTime)
	fmt.Println("Part 1:", answer)
}

func part1(input []byte) int {
	hands := parseInput(input)
	slices.SortStableFunc(hands, rankHands)
	answer := 0
	for i, hand := range hands {
		answer += hand.bid * (i + 1)
	}
	return answer
}

type Hand struct {
	cards    []int
	bid      int
	handType int
}

func rankHands(h1 Hand, h2 Hand) int {
	if h1.handType > h2.handType {
		return 1
	} else if h1.handType < h2.handType {
		return -1
	} else {
		for i := 0; i < len(h1.cards); i++ {
			if h1.cards[i] > h2.cards[i] {
				return 1
			} else if h1.cards[i] < h2.cards[i] {
				return -1
			}
		}
		return 0
	}
}

func getHandType(cards []int) int {
	counts := make([]int, 16)
	var handType int
	for _, card := range cards {
		counts[card]++
	}
	handType = High
	for _, c := range counts {
		if c == 5 {
			handType = Five
		} else if c == 4 {
			handType = Four
		} else if c == 3 {
			if handType == Pair {
				handType = Full
			} else {
				handType = Three
			}
		} else if c == 2 {
			if handType == Pair {
				handType = TwoPair
			} else if handType == Three {
				handType = Full
			} else {
				handType = Pair
			}
		}
	}
	return handType
}

const (
	Five    = 32
	Four    = 16
	Full    = 8
	Three   = 4
	TwoPair = 2
	Pair    = 1
	High    = 0
)

func parseInput(file []byte) []Hand {
	const a = (
		A = 15
		K = 14
		Q = 13
		J = 12
		T = 11
	)
	var hands []Hand
	for _, line := range bytes.Split(file, []byte("\n")) {
		fields := bytes.Fields(line)
		var cards []int
		for _, card := range fields[0] {
			if card == 'A' {
				cards = append(cards, A)
			} else if card == 'K' {
				cards = append(cards, K)
			} else if card == 'Q' {
				cards = append(cards, Q)
			} else if card == 'J' {
				cards = append(cards, J)
			} else if card == 'T' {
				cards = append(cards, T)
			} else {
				cards = append(cards, int(card-'0'))
			}
		}
		bid, _ := strconv.Atoi(string(fields[1]))
		handType := getHandType(cards)

		hands = append(hands, Hand{cards, bid, handType})
	}
	return hands
}
