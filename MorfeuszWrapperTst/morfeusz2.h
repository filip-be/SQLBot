/* 
 * File:   morfeusz2.h
 * Author: mlenart
 *
 * Created on 13 czerwiec 2014, 17:28
 */

#ifndef MORFEUSZ2_H
#define	MORFEUSZ2_H

#include <vector>
#include <string>
#include <list>
#include <set>

#ifndef __WIN32
#define DLLIMPORT
#else
/* A Windows system.  Need to define DLLIMPORT. */
#if BUILDING_MORFEUSZ
#define DLLIMPORT __declspec (dllexport)
#else
#define DLLIMPORT __declspec (dllimport)
#endif
#endif

namespace morfeusz {

    class DLLIMPORT MorphInterpretation;
    class DLLIMPORT Morfeusz;
    class DLLIMPORT ResultsIterator;
    class DLLIMPORT IdResolver;
    class DLLIMPORT MorfeuszException;

    enum Charset {
        UTF8 = 11,
        ISO8859_2 = 12,
        CP1250 = 13,
        CP852 = 14
    };

    enum TokenNumbering {
        /**
         * Start from 0. Reset counter for every invocation of Morfeusz::analyze (this is default)
         */
        SEPARATE_NUMBERING = 201,

        /**
         * Also start from 0. Reset counter for every invocation of Morfeusz::setTokenNumbering only
         */
        CONTINUOUS_NUMBERING = 202
    };
    
    enum CaseHandling {
        /**
         * Case-sensitive but allows interpretations that do not match case but there are no alternatives (this is default)
         */
        CONDITIONALLY_CASE_SENSITIVE = 100,

        /**
         * Strictly case-sensitive, reject all interpretations that do not match case
         */
        STRICTLY_CASE_SENSITIVE = 101,

        /**
         * Case-insensitive - ignores case
         */
        IGNORE_CASE = 102
    };

    enum WhitespaceHandling {
        /**
         * Ignore whitespaces (this is default)
         */
        SKIP_WHITESPACES = 301,

        /**
         * Append whitespaces to previous MorphInterpretation
         */
        APPEND_WHITESPACES = 302,

        /**
         * Whitespaces are separate MorphInterpretation objects
         */
        KEEP_WHITESPACES = 303
    };
    
    enum MorfeuszUsage {
        ANALYSE_ONLY = 401,
        GENERATE_ONLY = 402,
        BOTH_ANALYSE_AND_GENERATE = 403
    };

    /**
     * Performs morphological analysis (analyze methods) and syntesis (generate methods).
     * 
     * It is NOT thread-safe
     * but it is possible to use separate Morfeusz instance for each concurrent thread.
     */
    class DLLIMPORT Morfeusz {
    public:

        /**
         * Returns a string containing library version.
         * @return 
         */
        static std::string getVersion();
        
        /**
         * Returns a string containing default dictionary name.
         * @return 
         */
        static std::string getDefaultDictName();
        
        /**
         * Returns morfeusz2 library copyright text.
         * @return 
         */
        static std::string getCopyright();

        /**
         * Creates actual instance of Morfeusz class.
         * The caller is responsible for destroying it.
         * 
         * @remarks NOT THREAD-SAFE (affects ALL Morfeusz instances)
         * @return new instance of Morfeusz.
         */
        static Morfeusz* createInstance(MorfeuszUsage usage=BOTH_ANALYSE_AND_GENERATE);
        
        /**
         * Creates actual instance of Morfeusz class with possibly non-default dictionary.
         * The caller is responsible for destroying it.
         * 
         * @remarks NOT THREAD-SAFE (affects ALL Morfeusz instances)
         * @return new instance of Morfeusz.
         */
        static Morfeusz* createInstance(const std::string& dictName, MorfeuszUsage usage=BOTH_ANALYSE_AND_GENERATE);
        
        /**
         * Returns current dictionary ID.
         * 
         * @return dictionary ID string
         */
        virtual std::string getDictID() const = 0;
        
        /**
         * Returns current dictionary copyright string.
         * 
         * @return dictionary copyright string
         */
        virtual std::string getDictCopyright() const = 0;
        
        /**
         * Creates exact copy of Morfeusz object.
         * 
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         */
        virtual Morfeusz* clone() const = 0;

        virtual ~Morfeusz();

        /**
         * Analyze given text and return the results as iterator.
         * Use this method for analysis of big texts.
         * Copies the text under the hood - use analyze(const char*) if you want to avoid this.
         * 
         * @param text - text for morphological analysis.
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         * @return - iterator over morphological analysis results
         */
        virtual ResultsIterator* analyse(const std::string& text) const = 0;

        /**
         * Analyze given text and return the results as iterator.
         * It does not store results for whole text at once, so may be less memory-consuming for analysis of big texts
         * 
         * 
         * @param text - text for morphological analysis. This pointer must not be deleted before returned ResultsIterator object.
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         * @return - iterator over morphological analysis results
         */
        virtual ResultsIterator* analyse(const char* text) const = 0;

        /**
         * Perform morphological analysis on a given text and put results in a vector.
         * 
         * @param text - text to be analyzed
         * @param result - results vector
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         */
        virtual void analyse(const std::string& text, std::vector<MorphInterpretation>& result) const = 0;

        /**
         * Perform morphological synthesis on a given lemma and put results in a vector.
         * 
         * @param lemma - lemma to be analyzed
         * @param result - results vector
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         * @throws MorfeuszException - when lemma parameter contains whitespaces.
         */
        virtual void generate(const std::string& lemma, std::vector<MorphInterpretation>& result) const = 0;

        /**
         * Perform morphological synthesis on a given lemma and put results in a vector.
         * Limit results to interpretations with the specified tag.
         * 
         * @param lemma - lemma to be analyzed
         * @param tag - tag of result interpretations
         * @param result - results vector
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         * @throws MorfeuszException - when lemma parameter contains whitespaces or tagId is outside tagset.
         */
        virtual void generate(const std::string& lemma, int tagId, std::vector<MorphInterpretation>& result) const = 0;

        /**
         * Set encoding for input and output string objects.
         * 
         * @param encoding
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         */
        virtual void setCharset(Charset encoding) = 0;
        
        /**
         * Get charset used for input and output string objects.
         * @return 
         */
        virtual Charset getCharset() const = 0;

        /**
         * Select agglutination rules.
         * 
         * @param aggl
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         * @throws MorfeuszException - for invalid aggl parameter.
         */
        virtual void setAggl(const std::string& aggl) = 0;
        
        /**
         * Get current agglutination rules option
         * @return 
         */
        virtual std::string getAggl() const = 0;

        /**
         * Select past tense segmentation
         * 
         * @param praet
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         * @throws MorfeuszException - for invalid aggl praet parameter.
         */
        virtual void setPraet(const std::string& praet) = 0;
        
        /**
         * Get current past tense segmentation option
         * @return 
         */
        virtual std::string getPraet() const = 0;

        /**
         * Set case handling.
         * 
         * @param caseSensitive
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         */
        virtual void setCaseHandling(CaseHandling caseHandling) = 0;
        
        /**
         * Get case handling policy.
         * @return 
         */
        virtual CaseHandling getCaseHandling() const = 0;

        /**
         * Set token numbering policy.
         * 
         * @param numbering
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         */
        virtual void setTokenNumbering(TokenNumbering numbering) = 0;
        
        /**
         * Get token numbering policy.
         * @return 
         */
        virtual TokenNumbering getTokenNumbering() const = 0;

        /**
         * Set whitespace handling.
         * 
         * @param numbering
         * @remarks NOT THREAD-SAFE (must have exclusive access to this instance. Does not affect other Morfeusz instances).
         */
        virtual void setWhitespaceHandling(WhitespaceHandling whitespaceHandling) = 0;
        
        /**
         * Get whitespace handling.
         * @return 
         */
        virtual WhitespaceHandling getWhitespaceHandling() const = 0;

        /**
         * Set debug option value.
         * 
         * @param debug
         */
        virtual void setDebug(bool debug) = 0;

        /**
         * Get reference to tagset currently being in use.
         * 
         * @return currently used tagset
         */
        virtual const IdResolver& getIdResolver() const = 0;

        /**
         * Set current dictionary to the one with provided name.
         * 
         * This is NOT THREAD SAFE - no other thread may invoke setDictionary 
         * either within this instance, or any other in the same application.
         * 
         * @param dictName dictionary name
         * @remarks NOT THREAD-SAFE (affects ALL Morfeusz instances)
         * @throws MorfeuszException - when dictionary not found.
         * @throws std::ios_base::failure - when IO error occurred when loading given dictionary.
         */
        virtual void setDictionary(const std::string& dictName) = 0;

        /**
         * List of paths where current Morfeusz instance will look for dictionaries.
         * Modifying it is NOT THREAD-SAFE.
         */
        static std::list<std::string> dictionarySearchPaths;

        /**
         * Get available parameters for "setAggl" method.
         * @return 
         */
        virtual const std::set<std::string>& getAvailableAgglOptions() const = 0;

        /**
         * Get available parameters for "setPraet" method.
         * @return 
         */
        virtual const std::set<std::string>& getAvailablePraetOptions() const = 0;

    protected:
        /**
         * Same as analyze(text) but copies the text under the hood.
         * Useful for wrappers to other languages.
         */
        virtual ResultsIterator* analyseWithCopy(const char* text) const = 0;
    };

    class DLLIMPORT ResultsIterator {
    public:
        /**
         * 
         * @return true iff this iterator contains more elements.
         */
        virtual bool hasNext() = 0;
        
        /**
         * 
         * @return the element, that will be returned in next next() invocation.
         * @throws std::out_of_range when this iterator has already reached the end.
         */
        virtual const MorphInterpretation& peek() = 0;
        
        /**
         * 
         * @return next analysis result.
         * @throws std::out_of_range when this iterator has already reached the end.
         */
        virtual MorphInterpretation next() = 0;

        virtual ~ResultsIterator() {
        }
    };

    /**
     * Represents mappings for tags, names and labels.
     */
    class DLLIMPORT IdResolver {
    public:
        
        /**
         * Returns current TAGSET-ID (as specified in first line of tagset file)
         * 
         * @return tagset id string
         */
        virtual const std::string getTagsetId() const = 0;

        /**
         * Returns tag (denoted by its index).
         * 
         * @param tagNum - tag index in the tagset.
         * @return - the tag
         * @throws std::out_of_range when invalid tagId is provided.
         */
        virtual const std::string& getTag(const int tagId) const = 0;

        /**
         * Returns identifier for given tag.
         * Throws MorfeuszException when none exists.
         * 
         * @return identifier for given tag
         * @throws MorfeuszException when invalid tag parameter is provided.
         */
        virtual int getTagId(const std::string& tag) const = 0;

        /**
         * Returns named entity type (denoted by its index).
         * 
         * @param nameNum - name index in the tagset.
         * @return - the named entity type
         * @throws std::out_of_range when invalid nameId is provided.
         */
        virtual const std::string& getName(const int nameId) const = 0;

        /**
         * Returns identifier for given named entity.
         * Throws MorfeuszException when none exists.
         * 
         * @return identifier for given named entity
         * @throws MorfeuszException when invalid name parameter is provided.
         */
        virtual int getNameId(const std::string& name) const = 0;

        /**
         * Returns labels string for given labelsId.
         * 
         * @param labelsId
         * @return labels as string
         * @throws std::out_of_range when invalid labelsId is provided.
         */
        virtual const std::string& getLabelsAsString(int labelsId) const = 0;

        /**
         * Returns labels as set of strings for given labelsId.
         * @param labelsId
         * @return labels as set of strings
         * @throws std::out_of_range when invalid labelsId is provided.
         */
        virtual const std::set<std::string>& getLabels(int labelsId) const = 0;

        /**
         * Get labelsId for given labels as string.
         * 
         * @param labelsStr
         * @return labelsId
         * @throws MorfeuszException when invalid tag is provided.
         */
        virtual int getLabelsId(const std::string& labelsStr) const = 0;

        /**
         * Returns number of tags this tagset contains.
         * 
         * @return 
         */
        virtual size_t getTagsCount() const = 0;

        /**
         * Returns number of named entity types this tagset contains.
         * 
         * @return 
         */
        virtual size_t getNamesCount() const = 0;

        /**
         * Returns number of different labels combinations.
         */
        virtual size_t getLabelsCount() const = 0;

        virtual ~IdResolver() {
        }
    };

    /**
     The result of analysis is  a directed acyclic graph with numbered
     nodes representing positions  in text (points _between_ segments)
     and edges representing interpretations of segments that span from
     one node to another.  E.g.,

         {0,1,"ja","ja","ppron12:sg:nom:m1.m2.m3.f.n1.n2:pri"}
         |
         |      {1,2,"został","zostać","praet:sg:m1.m2.m3:perf"}
         |      |
       __|  ____|   __{2,3,"em","być","aglt:sg:pri:imperf:wok"}
      /  \ /     \ / \
     * Ja * został*em *
     0    1       2   3

     Note that the word 'zostałem' got broken into 2 separate segments.
     * One MorphInterpretation instance describes one edge of this DAG.
     */
    struct DLLIMPORT MorphInterpretation {
        
        MorphInterpretation()
        : startNode(0), endNode(0), orth(), lemma(), tagId(0), nameId(0), labelsId(0) {}
        
        /**
         * Creates new instance with "ign" tag (meaning: "not found in the dictionary")
         */
        static MorphInterpretation createIgn(
                int startNode, int endNode,
                const std::string& orth, const std::string& lemma);

        /**
         * Creates new instance with "sp" tag (meaning: "this is a sequence of whitespaces")
         */
        static MorphInterpretation createWhitespace(int startNode, int endNode, const std::string& orth);

        /**
         * 
         * @return true iff this instance represents an unknown word.
         */
        inline bool isIgn() const {
            return tagId == 0;
        }

        /**
         * 
         * @return true iff this instance represents a whitespace.
         */
        inline bool isWhitespace() const {
            return tagId == 1;
        }
        
        /**
         * Get tag as string.
         * 
         * @param morfeusz Morfeusz instance this interpretation was created by.
         * @return 
         */
        inline const std::string& getTag(const Morfeusz& morfeusz) const {
            return morfeusz.getIdResolver().getTag(this->tagId);
        }
        
        /**
         * Get name as string.
         * 
         * @param morfeusz Morfeusz instance this interpretation was created by.
         * @return 
         */
        inline const std::string& getName(const Morfeusz& morfeusz) const {
            return morfeusz.getIdResolver().getName(this->nameId);
        }
        
        /**
         * Get labels as string.
         * 
         * @param morfeusz Morfeusz instance this interpretation was created by.
         * @return 
         */
        inline const std::string& getLabelsAsString(const Morfeusz& morfeusz) const {
            return morfeusz.getIdResolver().getLabelsAsString(this->labelsId);
        }
        
        /**
         * Get tag as set of strings.
         * 
         * @param morfeusz Morfeusz instance this interpretation was created by.
         * @return 
         */
        inline const std::set<std::string>& getLabels(const Morfeusz& morfeusz) const {
            return morfeusz.getIdResolver().getLabels(this->labelsId);
        }

        int startNode;
        int endNode;
        std::string orth;
        std::string lemma;
        int tagId;
        int nameId;
        int labelsId;
    };

    class DLLIMPORT MorfeuszException : public std::exception {
    public:

        MorfeuszException(const std::string& what) : msg(what.c_str()) {
        }

        virtual ~MorfeuszException() throw () {
        }

        virtual const char* what() const throw () {
            return this->msg.c_str();
        }
    private:
        const std::string msg;
    };

    class DLLIMPORT FileFormatException : public MorfeuszException {
    public:

        FileFormatException(const std::string& what) : MorfeuszException(what) {
        }
    };
}

#endif	/* MORFEUSZ2_H */

