package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
	"time"
	"unicode"
)

func main() {
	data, err := os.ReadFile("example2.txt")
	if err != nil {
		fmt.Println(err.Error())
	}
	input := strings.Split(string(data), "\n")

	//only 12 red cubes, 13 green cubes, and 14 blue cubes
	criteria := []int{12, 13, 14}

	fmt.Println("Part 1 - pointer to string")
	startTime := time.Now()
	sum := 0
	for id, game := range input {
		if isGamePossible(game, criteria) {
			sum += id + 1
		} else {
		}
	}
	fmt.Println(sum)
	endTime := time.Now()
	elapsedTime := endTime.Sub(startTime)
	fmt.Println("elapsed time: ", elapsedTime)

	fmt.Println("Part 2 - string manipulation")
	startTime = time.Now()
	sum = 0
	for id, game := range input {
		if isGamePossible2(game, criteria) {
			sum += id + 1
		}
	}
	fmt.Println(sum)
	endTime = time.Now()
	elapsedTime = endTime.Sub(startTime)
	fmt.Println("elapsed time: ", elapsedTime)

	fmt.Println("Part 3 - power set of min")
	startTime = time.Now()
	powerSum := 0
	for _, game := range input {
		powerSum += powerSetOfMin(game)
	}
	endTime = time.Now()
	elapsedTime = endTime.Sub(startTime)
	fmt.Println("elapsed time: ", elapsedTime)
	fmt.Println(powerSum)
}

func isGamePossible(game string, criteria []int) bool {
	chars := []rune(game)
	i := 0
	for chars[i] != ':' {
		i++
	}
	i++
	for i <= len(chars)-1 {
		var gameState [3]int
		for i <= len(chars)-1 && chars[i] != ';' {
			for i <= len(chars)-1 && isSpaceOrPunc(chars[i]) {
				i++
			}
			var sb strings.Builder
			for i <= len(chars)-1 && unicode.IsDigit(chars[i]) {
				sb.WriteRune(chars[i])
				i++
			}
			digit, _ := strconv.Atoi(sb.String())
			sb.Reset()
			for i <= len(chars)-1 && isSpaceOrPunc(chars[i]) {
				i++
			}
			for i <= len(chars)-1 && unicode.IsLetter(chars[i]) {
				sb.WriteRune(chars[i])
				i++
			}
			color := sb.String()
			switch color {
			case "red":
				gameState[0] += digit
			case "green":
				gameState[1] += digit
			case "blue":
				gameState[2] += digit
			}
			for i, color := range gameState {
				if color > criteria[i] {
					return false
				}
			}
		}
		i++
	}
	return true
}

func isSpaceOrPunc(c rune) bool {
	return unicode.IsSpace(c) || unicode.IsPunct(c)
}

func isGamePossible2(game string, criteria []int) bool {
	game = strings.ReplaceAll(game, " ", "")
	game = strings.SplitAfter(game, ":")[1]
	rounds := strings.Split(game, ";")
	for _, round := range rounds {
		sets := strings.Split(round, ",")
		var gameState [3]int
		for _, set := range sets {
			li := strings.IndexFunc(set, unicode.IsLetter)
			digit, _ := strconv.Atoi(set[:li])
			switch set[li:] {
			case "red":
				gameState[0] += digit
			case "green":
				gameState[1] += digit
			case "blue":
				gameState[2] += digit
			}
			for i, count := range gameState {
				if count > criteria[i] {
					return false
				}
			}
		}
	}
	return true
}

func powerSetOfMin(game string) int {
	game = strings.ReplaceAll(game, " ", "")
	game = strings.SplitAfter(game, ":")[1]
	rounds := strings.Split(game, ";")
	var mins [3]int
	for _, round := range rounds {
		sets := strings.Split(round, ",")
		for _, set := range sets {
			li := strings.IndexFunc(set, unicode.IsLetter)
			digit, _ := strconv.Atoi(set[:li])
			color := set[li:]
			switch color {
			case "red":
				mins[0] = max(mins[0], digit)
			case "green":
				mins[1] = max(mins[1], digit)
			case "blue":
				mins[2] = max(mins[2], digit)
			}
		}
	}
	power := 1
	for _, m := range mins {
		power *= m
	}
	return power
}
