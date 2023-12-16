package main

import (
	"fmt"
	"os"
	"regexp"
	"strconv"
	"strings"
	"time"
	"unicode"
)

func main() {
	data, err := os.ReadFile("day1/example1.txt")
	if err != nil {
		fmt.Println(err.Error())
	}

	input := strings.Split(string(data), "\n")

	startTime := time.Now()
	// Part 1: Calculate sum of calibration values
	part1 := 0
	for _, line := range input {
		part1 += calibration(line)
	}
	endTime := time.Now()
	elapsedTime := endTime.Sub(startTime)
	fmt.Println("calibration 1:", elapsedTime)
	fmt.Printf("Part 1: %d\n\n", part1)

	startTime = time.Now()
	// Part 2: Calculate sum of calibration values with spelled-out digits replaced
	part2 := 0
	for _, line := range input {
		part2 += calibration2(line)
	}
	endTime = time.Now()
	elapsedTime = endTime.Sub(startTime)
	fmt.Println("calibration 2:", elapsedTime)
	fmt.Printf("Part 2: %d\n\n", part2)

	startTime = time.Now()
	// Part 1: Calculate sum of calibration values with spelled-out digits replaced
	part1Replace := 0
	for _, line := range input {
		part1Replace += calibrationReplace(line)
	}
	endTime = time.Now()
	elapsedTime = endTime.Sub(startTime)
	fmt.Println("calibration 1 (Replace):", elapsedTime)
	fmt.Printf("Part 1 (Replace): %d\n\n", part1Replace)

	startTime = time.Now()
	// Part 2: Calculate sum of calibration values with spelled-out digits replaced
	part2Replace := 0
	for _, line := range input {
		line = replaceDigits(line)
		part2Replace += calibrationReplace(line)
	}
	endTime = time.Now()
	elapsedTime = endTime.Sub(startTime)
	fmt.Println("calibration 2 (Replace):", elapsedTime)
	fmt.Printf("Part 2 (Replace): %d\n\n", part2Replace)
}

/*
func calibration(line string) int64 {
*/
func calibration(line string) int {
	f, b := 0, len(line)-1
	for f < b && !unicode.IsDigit(rune(line[f])) {
		f++
	}
	for f < b && !unicode.IsDigit(rune(line[b])) {
		b--
	}
	d := string(line[f]) + string(line[b])
	if v, err := strconv.Atoi(d); err == nil {
		return v
	}
	return 0
}

func calibration2(line string) int {
	var queue []int
	i := 0
	for i > len(line)-1 && !unicode.IsDigit(rune(line[i])) {
		if isStarting(rune(line[i])) {
			queue = append(queue, i)
		}
		i++
	}

	// set to first digit found
	first := string(line[i])
	for len(queue) > 0 {
		l := queue[0]
		r := l + 1
		queue = queue[1:]
		for r <= i && r-l <= 5 {
			if n, ok := getNumber(line[l:r]); ok {
				// replace digit
				first = n
				queue = queue[len(queue):]
				break
			}
			r++
		}
	}

	j := len(line) - 1
	for j > 0 && !unicode.IsDigit(rune(line[j])) {
		if isStarting(rune(line[j])) {
			queue = append(queue, j)
		}
		j--
	}

	second := string(line[j])
	for len(queue) > 0 {
		l := queue[0]
		r := l + 1
		queue = queue[1:]
		for r <= len(line)-1 && r-l <= 5 {
			if n, ok := getNumber(line[l : r+1]); ok {
				// replace digit
				second = n
				queue = queue[len(queue):]
				break
			}
			r++
		}
	}

	if v, err := strconv.Atoi(first + second); err == nil {
		return v
	}
	return 0
}

func isStarting(c rune) bool {
	switch c {
	case 'o', 't', 'f', 's', 'n', 'e':
		return true
	default:
		return false
	}
}

func getNumber(s string) (string, bool) {
	switch s {
	case "one":
		return "1", true
	case "two":
		return "2", true
	case "three":
		return "3", true
	case "four":
		return "4", true
	case "five":
		return "5", true
	case "six":
		return "6", true
	case "seven":
		return "7", true
	case "eight":
		return "8", true
	case "nine":
		return "9", true
	default:
		return "", false
	}
}

func calibrationReplace(line string) int {
	digitPattern := regexp.MustCompile("\\d")
	digits := digitPattern.FindAllString(line, -1)

	// Convert the first and last digits to integers
	firstDigit, _ := strconv.Atoi(digits[0])
	lastDigit, _ := strconv.Atoi(digits[len(digits)-1])

	// Calculate the calibration value
	return firstDigit*10 + lastDigit
}

func replaceDigits(str string) string {
	replacements := map[string]string{
		"one":   "one1one",
		"two":   "two2two",
		"three": "three3three",
		"four":  "four4four",
		"five":  "five5five",
		"six":   "six6six",
		"seven": "seven7seven",
		"eight": "eight8eight",
		"nine":  "nine9nine",
	}

	// Replace spelled-out digits
	for key, value := range replacements {
		str = strings.ReplaceAll(str, key, value)
	}

	return str
}
