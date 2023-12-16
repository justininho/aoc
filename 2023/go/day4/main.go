package main

import (
	"fmt"
	"math"
	"os"
	"slices"
	"strings"
)

func main() {
	input, err := os.ReadFile("example4.txt")
	if err != nil {
		panic(err.Error())
	}
	s := string(input)
	part1Total := part1(s)
	fmt.Println("part1:", part1Total)

	part2Total := part2(s)
	fmt.Println("part2:", part2Total)
}

func part1(s string) int {
	total := 0
	scratchcards := strings.Split(s, "\n")
	for _, card := range scratchcards {
		_, card, _ = strings.Cut(card, ":")
		points := processCard(card)
		total += int(math.Pow(2, float64(points-1)))
	}
	return total
}

func processCard(card string) int {
	nums := strings.Split(card, "|")
	winningNums := strings.Split(nums[0], " ")
	cardsNums := strings.Split(nums[1], " ")
	points := 0
	for _, n := range cardsNums {
		if n != "" && slices.Contains(winningNums, n) {
			points++
		}
	}
	return points
}

func part2(s string) int {
	var queue []int
	mp := make(map[int][]int)
	scratchcards := strings.Split(s, "\n")
	total := 0
	for i, card := range scratchcards {
		total++
		newCards, has := processCard2(i, card)
		if has {
			mp[i] = newCards
			queue = append(queue, newCards...)
		}
	}
	for len(queue) > 0 {
		total++
		card := queue[0]
		queue = queue[1:]
		if newCards, has := mp[card]; has {
			queue = append(queue, newCards...)
		}
	}
	return total
}

func processCard2(id int, card string) ([]int, bool) {
	nums := strings.Split(card, "|")
	winningNums := strings.Split(nums[0], " ")
	cardsNums := strings.Split(nums[1], " ")
	points := 0
	var cards []int
	for _, n := range cardsNums {
		if n != "" && slices.Contains(winningNums, n) {
			points++
			cards = append(cards, id+points)
		}
	}
	if len(cards) > 0 {
		return cards, true
	} else {
		return cards, false
	}
}
